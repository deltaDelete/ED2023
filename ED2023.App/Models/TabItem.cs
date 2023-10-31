using System;
using Avalonia;
using Avalonia.Controls;

namespace ED2023.App.Models; 

// public record class TabItem {
//     public string Header { get; set; }
//     public Type PageType { get; set; }
// }

public record class TabItem(string Header, Control Content);