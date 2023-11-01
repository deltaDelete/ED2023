using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ED2023.App.Views;
using ED2023.Database;
using ED2023.Database.Models;
using ReactiveUI;

namespace ED2023.App.ViewModels;

public partial class TableViewModelBase<T> : TableViewModelBase {
    private List<T> _itemsFull = null!;

    private readonly Func<List<T>> _databaseGetter;

    /// <summary>
    /// Словарь, где ключ это индекс колонки, а значение селектор этой колонки
    /// </summary>
    private readonly Dictionary<int, Func<T, object>> _orderSelectors;

    /// <summary>
    /// Дефорлтный селектор на случай если не будет найдено другого 
    /// </summary>
    private readonly Func<T, object> _defaultOrderSelector;

    /// <summary>
    /// Словарь, где ключ это индекс колонки, а значение функция возвращающая булево при нужном условии
    /// </summary>
    private readonly Dictionary<int, Func<string, Func<T, bool>>> _filterSelectors;

    /// <summary>
    /// Дефорлтный селектор на случай если не будет найдено другого 
    /// </summary>
    private readonly Func<string, Func<T, bool>> _defaultFilterSelector;

    private readonly Action<T?> _editItem;
    private readonly Func<Task> _newItem;
    private readonly Action<T?> _removeItem;
    private BindingList<T> _items = new();
    private List<T> _filtered = new List<T>();
    private T? _selectedRow = default;

    #region Notifying Properties

    // TODO: ЯВНЫЕ БЭКФИЛДЫ
    public new BindingList<T> Items {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public new List<T> Filtered {
        get => _filtered;
        set => this.RaiseAndSetIfChanged(ref _filtered, value);
    }

    public new T? SelectedRow {
        get => _selectedRow;
        set => this.RaiseAndSetIfChanged(ref _selectedRow, value);
    }

    #endregion

    public new ReactiveCommand<T, Unit> EditItemCommand { get; }
    public new ReactiveCommand<T, Unit> RemoveItemCommand { get; }
    public new ReactiveCommand<Unit, Unit> NewItemCommand { get; }
    public new ReactiveCommand<Unit, Unit> TakeNextCommand { get; }
    public new ReactiveCommand<Unit, Unit> TakePrevCommand { get; }
    public new ReactiveCommand<Unit, Unit> TakeFirstCommand { get; }
    public new ReactiveCommand<Unit, Unit> TakeLastCommand { get; }

    public TableViewModelBase(
        Func<List<T>> databaseGetter,
        Dictionary<int, Func<T, object>> orderSelectors,
        Func<T, object> defaultOrderSelector,
        Dictionary<int, Func<string, Func<T, bool>>> filterSelectors,
        Func<string, Func<T, bool>> defaultFilterSelector, Action<T?> editItem, Func<Task> newItem,
        Action<T?> removeItem) {
        _databaseGetter = databaseGetter;
        _orderSelectors = orderSelectors;
        _defaultOrderSelector = defaultOrderSelector;
        _filterSelectors = filterSelectors;
        _defaultFilterSelector = defaultFilterSelector;
        _editItem = editItem;
        _newItem = newItem;
        _removeItem = removeItem;

        var canTakeNext = this.WhenAnyValue(
            x => x.CurrentPage,
            selector: it => it < TotalPages);
        var canTakeBack = this.WhenAnyValue(
            x => x.CurrentPage,
            selector: it => it > 1);
        var canTakeLast = this.WhenAnyValue(
            x => x.CurrentPage,
            x => x.TotalPages,
            selector: (i1, i2) => i1 < i2);

        // var canEdit = this.WhenAnyValue(
        //     x => x.SelectedRow,
        //     selector: client => client is not null
        //                         && MainWindow.CurrentUserGroups
        //                                      .Any(it => it.Permissions.HasFlag(Permissions.Write)));
        //
        // var canInsert = MainWindow.WhenAnyValue(
        //     it => it.CurrentUserGroups,
        //     selector: it => it.Any(group => group.Permissions.HasFlag(Permissions.Insert))
        // );

        TakeNextCommand = ReactiveCommand.Create(TakeNext, canTakeNext);
        TakePrevCommand = ReactiveCommand.Create(TakePrev, canTakeBack);
        TakeFirstCommand = ReactiveCommand.Create(TakeFirst, canTakeBack);
        TakeLastCommand = ReactiveCommand.Create(TakeLast, canTakeLast);
        EditItemCommand = ReactiveCommand.Create<T>(_editItem); //, canEdit);
        RemoveItemCommand = ReactiveCommand.Create<T>(_removeItem); // , canEdit);
        NewItemCommand = ReactiveCommand.CreateFromTask(_newItem); //, canInsert);

        GetDataFromDb();

        this.WhenAnyValue(
                x => x.SearchQuery,
                x => x.SelectedSearchColumn,
                x => x.IsSortByDescending
            )
            .DistinctUntilChanged()
            .Subscribe(OnSearchChanged);
        this.WhenAnyValue(
            x => x.Filtered
        ).Subscribe(_ => TakeFirst());

        // this.WhenAnyValue(
        //     x => x.Items
        // ).Subscribe(_ => IsLoading = false);
    }

    private void OnSearchChanged((string query, int column, bool isDescending) tuple) {
        if (_itemsFull is null) {
            return;
        }

        var filtered = string.IsNullOrWhiteSpace(tuple.query)
            ? _itemsFull
            : _itemsFull.Where(
                _filterSelectors.GetValueOrDefault(tuple.column, _defaultFilterSelector)(tuple.query.ToLower()));

        Filtered = tuple.isDescending switch {
            (true) => filtered.OrderByDescending(_orderSelectors.GetValueOrDefault(tuple.column, _defaultOrderSelector))
                .ToList(),
            (false) => filtered.OrderBy(_orderSelectors.GetValueOrDefault(tuple.column, _defaultOrderSelector))
                .ToList(),
        };
    }

    private async void GetDataFromDb() {
        await Task.Run(async () => {
            IsLoading = true;
            if (_databaseGetter is null) {
                throw new NullReferenceException();
            }

            var list = _databaseGetter?.Invoke();
            _itemsFull = list ?? new List<T>();
            Filtered = _itemsFull;
            IsLoading = false;
            SearchQuery = string.Empty;
            return Task.CompletedTask;
        });
    }

    // protected async void RemoveItem(T? arg) {
    //     if (arg is null) return;
    //     // await new ConfirmationDialog(
    //     //     "Вы собираетесь удалить строку",
    //     //     $"Пользователь: {arg.LastName} {arg.FirstName} {arg.MiddleName}",
    //     //     async dialog =>
    //     //     {
    //     //         await using var db = new MyDatabase();
    //     //         await db.RemoveAsync(arg);
    //     //         RemoveLocal(arg);
    //     //     },
    //     //     dialog => { }
    //     // ).ShowDialog(_clientView);
    //     throw new NotImplementedException();
    // }

    protected void RemoveLocal(T arg) {
        Items.Remove(arg);
        _itemsFull.Remove(arg);
        Filtered.Remove(arg);
    }

    // private async void EditItem(T? arg) {
    //     if (arg is null) return;
    //     await EditDialog.NewInstance(
    //         async item => {
    //             await using var db = new MyDatabase();
    //             await db.UpdateAsync(item.ClientId, item);
    //             ReplaceItem(arg, item);
    //         },
    //         arg,
    //         title: "Изменить клиента"
    //     ).ShowDialog(MainWindow);
    //     // throw new NotImplementedException();
    // }

    // private async Task NewItem() {
    //     await EditDialog.NewInstance<T>(
    //         async item => {
    //             await using var db = new MyDatabase();
    //             int newItemId = Convert.ToInt32(await db.InsertAsync(item));
    //             item.ClientId = newItemId;
    //             _itemsFull.Add(item);
    //             if (Items.Count < 10) {
    //                 Items.Add(item);
    //             }
    //         },
    //         title: "Новый клиент"
    //     ).ShowDialog(MainWindow);
    // }

    protected void ReplaceItem(T prevItem, T newItem) {
        if (Filtered.Contains(prevItem)) {
            var index = Filtered.IndexOf(prevItem);
            Filtered[index] = newItem;
        }

        if (_itemsFull.Contains(prevItem)) {
            var index = _itemsFull.IndexOf(prevItem);
            _itemsFull[index] = newItem;
        }

        if (Items.Contains(prevItem)) {
            var index = _itemsFull.IndexOf(prevItem);
            Items[index] = newItem;
        }
    }

    protected void TakeNext() {
        Skip += Take;
        Items = new(
            Filtered.Skip(Skip).Take(Take).ToList()
        );
    }

    protected void TakePrev() {
        Skip -= Take;
        Items = new(
            Filtered.Skip(Skip).Take(Take).ToList()
        );
    }

    protected void TakeFirst() {
        Skip = 0;
        Items = new(
            Filtered.Take(Take).ToList()
        );
    }

    protected void TakeLast() {
        Skip = Filtered.Count - Take;
        Items = new(
            Filtered.TakeLast(Take).ToList()
        );
    }
}

public abstract class TableViewModelBase : ViewModelBase {
    private int _selectedSearchColumn;
    private bool _isSortByDescending = false;
    private string _searchQuery = string.Empty;
    private int _take = 10;
    private int _skip = 0;
    private int _currentPage = 1;
    private bool _isLoading = true;

    public ReactiveCommand<object, Unit> EditItemCommand { get; }
    public ReactiveCommand<object, Unit> RemoveItemCommand { get; }
    public ReactiveCommand<Unit, Unit> NewItemCommand { get; }
    public ReactiveCommand<Unit, Unit> TakeNextCommand { get; }
    public ReactiveCommand<Unit, Unit> TakePrevCommand { get; }
    public ReactiveCommand<Unit, Unit> TakeFirstCommand { get; }
    public ReactiveCommand<Unit, Unit> TakeLastCommand { get; }


    public int SelectedSearchColumn {
        get => _selectedSearchColumn;
        set => this.RaiseAndSetIfChanged(ref _selectedSearchColumn, value);
    }

    public bool IsSortByDescending {
        get => _isSortByDescending;
        set => this.RaiseAndSetIfChanged(ref _isSortByDescending, value);
    }

    public string SearchQuery {
        get => _searchQuery;
        set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
    }


    public int Take {
        get => _take;
        set => this.RaiseAndSetIfChanged(ref _take, value);
    }

    public int Skip {
        get => _skip;
        set => this.RaiseAndSetIfChanged(ref _skip, value);
    }

    public int CurrentPage {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public bool IsLoading {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public int TotalPages {
        get {
            var val = (int)Math.Ceiling(Filtered.Count / (double)Take);
            if (val <= 0) val = 1;
            return val;
        }
    }

    public List<object> Filtered { get; set; } = new List<object>();
    public BindingList<object> Items { get; set; } = new();
    public object? SelectedRow { get; set; } = default;
}