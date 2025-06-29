using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Reactive;
using FinnishApp.Models;
using System.Linq;

namespace FinnishApp.ViewModels
{
    public class FlashcardsStudyViewModel : ReactiveObject
    {
        public string Title { get; }
        public ObservableCollection<WordPair> Words { get; } = new();
        int _index = 0;
        bool _flipped = false;
        public ReactiveCommand<Unit, Unit> ShuffleCommand { get; }

        public string DisplayText =>
            Words.Count == 0
                ? ""
                : !_flipped
                    ? Words[_index].Polish
                    : Words[_index].Finnish;
        
        public string Progress =>
            Words.Count == 0
                ? "0/0"
                : $"{_index + 1}/{Words.Count}";

        public string FlipButtonText =>
            !_flipped ? "Pokaż tłumaczenie" : "Ukryj tłumaczenie";

        public bool CanPrevious => _index > 0;
        public bool CanNext     => _index < Words.Count - 1;

        public ReactiveCommand<Unit, Unit> FlipCommand     { get; }
        public ReactiveCommand<Unit, Unit> NextCommand     { get; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand     { get; }
        public event Action? RequestBack;

        public FlashcardsStudyViewModel(string category, string fileName)
        {
            string cleaned = category.TrimStart(' ', '-');
            Title = $"Fiszki: {cleaned}";

            // Wczytanie danych z JSON
            var path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var arr  = JsonSerializer.Deserialize<WordPair[]>(json)
                           ?? Array.Empty<WordPair>();
                foreach (var w in arr)
                    Words.Add(w);
            }

            // Komenda flip
            FlipCommand = ReactiveCommand.Create(() =>
            {
                _flipped = !_flipped;
                NotifyAll();
                this.RaisePropertyChanged(nameof(DisplayText));
                this.RaisePropertyChanged(nameof(FlipButtonText));
            });

            // Komenda „Następna”
            NextCommand = ReactiveCommand.Create(() =>
            {
                if (_index < Words.Count - 1)
                {
                    _index++;
                    _flipped = false;
                    NotifyAll();
                    this.RaisePropertyChanged(nameof(DisplayText));
                    this.RaisePropertyChanged(nameof(FlipButtonText));   // <- dodane
                    this.RaisePropertyChanged(nameof(CanPrevious));
                    this.RaisePropertyChanged(nameof(CanNext));
                }
            });

            // Komenda „Poprzednia”
            PreviousCommand = ReactiveCommand.Create(() =>
            {
                if (_index > 0)
                {
                    _index--;
                    _flipped = false;
                    NotifyAll();
                    this.RaisePropertyChanged(nameof(DisplayText));
                    this.RaisePropertyChanged(nameof(FlipButtonText));   // <- dodane
                    this.RaisePropertyChanged(nameof(CanPrevious));
                    this.RaisePropertyChanged(nameof(CanNext));
                }
            });

            // Komenda Cofnij
            BackCommand = ReactiveCommand.Create(() => RequestBack?.Invoke());
            
            ShuffleCommand = ReactiveCommand.Create(() =>
            {
                if (Words.Count <= 1)
                    return;

                // prosty shuffle przez OrderBy
                var rnd = new Random();
                var shuffled = Words.OrderBy(_ => rnd.Next()).ToList();

                // odświeżamy kolekcję
                Words.Clear();
                foreach (var w in shuffled)
                    Words.Add(w);

                // reset stanu
                _index = 0;
                _flipped = false;

                // powiadamiamy o zmianach
                NotifyAll();
                this.RaisePropertyChanged(nameof(DisplayText));
                this.RaisePropertyChanged(nameof(FlipButtonText));
                this.RaisePropertyChanged(nameof(CanPrevious));
                this.RaisePropertyChanged(nameof(CanNext));
            });
            
            void NotifyAll()
            {
                this.RaisePropertyChanged(nameof(DisplayText));
                this.RaisePropertyChanged(nameof(FlipButtonText));
                this.RaisePropertyChanged(nameof(CanPrevious));
                this.RaisePropertyChanged(nameof(CanNext));
                this.RaisePropertyChanged(nameof(Progress));
            }
        }
    }
}
