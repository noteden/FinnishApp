using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace FinnishApp.Views.Grammar;

public partial class VerbsDeclPage : UserControl
{
    // eventy do nawigacji
    public event Action? BackClicked;
    public event Action<string>? CategoryClicked;

    // Twoje kategorie odmiany
    private readonly List<string> _categories = new()
    {
        "1. Typy czasowników",
        "2. Czas przeszły"
    };

    public VerbsDeclPage()
    {
        InitializeComponent();

        // Hook przycisku Cofnij
        this.FindControl<Button>("BackButton")
            .Click += (_,__) => BackClicked?.Invoke();

        // Wypełnij ItemsPanel
        var panel = this.FindControl<StackPanel>("ItemsPanel");
        foreach (var cat in _categories)
        {
            var txt = new TextBlock
            {
                Text       = cat,
                FontSize   = 18,
                Cursor     = new Cursor(StandardCursorType.Hand),
                Foreground = new SolidColorBrush(Color.Parse("#22c55e")),
                Margin     = new Avalonia.Thickness(0, 0, 0, 6)
            };
            txt.PointerPressed += (_, __) => CategoryClicked?.Invoke(cat);
            panel.Children.Add(txt);
        }
    }

    private void InitializeComponent() 
        => AvaloniaXamlLoader.Load(this);
}