using Avalonia.Collections;

using System;
using System.Linq;
using ReactiveUI;

public class MainViewModel : ReactiveObject
{
    private AvaloniaList<Item> _items;
    private string _searchText;

    public MainViewModel()
    {
        // Инициализация данных для таблицы
        Items = new AvaloniaList<Item>
        {
            new Item { Name = "Item 1", Description = "Description 1" },
            new Item { Name = "Item 2", Description = "Description 2" },
            new Item { Name = "Item 3", Description = "Description 3" },
        };
        this.WhenAnyValue(x => x.SearchText)
            .Subscribe(FilterItems);
    }

    public AvaloniaList<Item> Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    private void FilterItems(string searchText)
    {
        var filteredItems = _items.Where(item =>
                                            item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                            item.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
        Items = new AvaloniaList<Item>(filteredItems);
    }
}

public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
}
