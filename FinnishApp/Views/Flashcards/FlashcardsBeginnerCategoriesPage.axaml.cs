using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using Avalonia;

namespace FinnishApp.Views
{
    public partial class FlashcardsBeginnerCategoriesPage : UserControl
    {
        public event Action? BackClicked;
        public event Action<string>? CategoryClicked;

        private readonly Dictionary<string, List<string>> _groups = new()
        {
            ["1. Podstawowe wyrażenia:"] = new() { " - Fińskie zapożyczenia", " - Fiński do przetrwania", " - W klasie", " - Pierwsze 100 czasowników", " - Kraje", " - Narodowości", " - Języki", " - Liczby" },
            ["2. Przymiotniki:"]       = new() { " - Kolory", " - Przymiotniki i przeciwieństwa" },
            ["3. Czas:"]       = new() { " - Określenie czasu", " - Dni tygodnia", " - Miesiące" },
            ["4. Jedzenie:"]       = new() { " - Śniadanie", " - Warzywa", " - Owoce", " - Mięsa" },
            ["5. Ciało i zdrowie:"]       = new() { " - Części ciała", " - Kości", " - Choroby", " - U lekarza" },
            ["6. Zwierzęta:"]       = new() { " - Ptaki", " - Ryby i morskie stworzenia", " - Inskety", " - Zwierzęta domowe" },
            ["7. Jak powiedzieć?:"]       = new() { " - Dziękuje", " - Tak i Nie", " - Nie wiem", " - Jak się masz?", " - Mam...", " - Muszę...", " - Lubię..."},
            ["8. Inne:"]       = new() { " - Ubrania", " - Szkoła", " - Pogoda", " - Miejsca w mieście", " - Pokoje i meble", " - Członkowie rodziny" }
        };

        public FlashcardsBeginnerCategoriesPage()
        {
            InitializeComponent();

            // hook „Cofnij”
            this.FindControl<Button>("BackButton")
                .Click += (_,__) => BackClicked?.Invoke();

            var panel = this.FindControl<StackPanel>("ItemsPanel");
            foreach (var kv in _groups)
            {
                panel.Children.Add(new TextBlock
                {
                    Text       = kv.Key,
                    FontWeight = FontWeight.Bold,
                    FontSize   = 18,
                    Margin     = new Thickness(0,5)
                });

                foreach (var name in kv.Value)
                {
                    var txt = new TextBlock
                    {
                        Text       = name,
                        FontSize   = 16,
                        Cursor     = new Cursor(StandardCursorType.Hand),
                        Foreground = new SolidColorBrush(Color.Parse("#22c55e")),
                        Margin     = new Thickness(20,2)
                    };
                    txt.PointerPressed += (_,__) => CategoryClicked?.Invoke(name);
                    panel.Children.Add(txt);
                }
            }
        }

        private void InitializeComponent() 
            => AvaloniaXamlLoader.Load(this);
    }
}