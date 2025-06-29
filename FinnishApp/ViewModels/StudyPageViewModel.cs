using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Reactive;
using FinnishApp.Models;

namespace FinnishApp.ViewModels
{
    public class StudyPageViewModel : ReactiveObject
    {
        string _windowTitle = "";
        public string WindowTitle
        {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }

        // Lista par słówek
        public ObservableCollection<WordPair> WordPairs { get; } = new();

        // Komenda cofania
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        // Event do MainWindow
        public event Action? RequestBack;

        public StudyPageViewModel()
        {
            BackCommand = ReactiveCommand.Create(() => RequestBack?.Invoke());
        }

        public void LoadCategory(string category)
        {
            string cleaned = category.TrimStart(' ', '-');
            WindowTitle = $"Słówka: {cleaned}";
            WordPairs.Clear();
            
            var fileName = $"{category}.json";
            var path     = Path.Combine(AppContext.BaseDirectory, "Data", fileName);

            if (!File.Exists(path))
                return;

            var json = File.ReadAllText(path);
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            // JSON to tablica WordPair: [ { Polish, Finnish }, ... ]
            var list = JsonSerializer.Deserialize<WordPair[]>(json, opts)
                       ?? Array.Empty<WordPair>();

            foreach (var wp in list)
                WordPairs.Add(wp);
        }
    }
}