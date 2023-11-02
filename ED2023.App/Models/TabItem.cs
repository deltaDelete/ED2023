using System;
using Avalonia;
using Avalonia.Controls;

namespace ED2023.App.Models;

public class TabItem {
    public string Header { get; set; }
    public Control? Content { get; set; }

    public string? Text {
        get => Content is TextBlock tb ? tb.Text : null;
        set {
            if (Content is null) {
                Content = new TextBlock() {
                    Text = value
                };
                return;
            }

            if (Content is TextBlock tb) {
                tb.Text = value;
            }
        }
    }
}