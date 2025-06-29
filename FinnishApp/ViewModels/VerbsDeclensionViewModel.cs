using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Reactive;
using FinnishApp.Models;

namespace FinnishApp.ViewModels
{
    public class VerbsDeclensionViewModel : ReactiveObject
    {
        public string Title { get; }
        public ObservableCollection<string> Paragraphs { get; } = new();

        public ReactiveCommand<Unit, Unit> BackCommand { get; }
        public event Action? RequestBack;

        public VerbsDeclensionViewModel(string category, string fileName)
        {
            string cleaned = category.TrimStart(' ', '-');
            Title = $"Odmiana czasowników: {cleaned}";

            var path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<VerbsDeclData>(json,
                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                           ?? new VerbsDeclData();

                // wypełniamy akapity
                foreach (var p in data.Paragraphs)
                    Paragraphs.Add(p);
            }

            BackCommand = ReactiveCommand.Create(() => RequestBack?.Invoke());
        }
    }
}