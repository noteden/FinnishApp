using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Reactive;
using FinnishApp.Models;

namespace FinnishApp.ViewModels
{
    public class TestsPageViewModel : ReactiveObject
    {
        public string Title { get; }

        private readonly WordPair[] _words;
        private int _index = 0;

        // 1) Dodajemy pole przechowujące odpowiedź użytkownika:
        private string _userAnswer = "";
        public string UserAnswer
        {
            get => _userAnswer;
            set
            {
                // podstawa: aktualizacja pola i powiadomienie ReactiveUI
                this.RaiseAndSetIfChanged(ref _userAnswer, value);
                // **dodatkowo** powiadamiamy o zmianie IsAnswerNotEmpty
                this.RaisePropertyChanged(nameof(IsAnswerNotEmpty));
            }
        }

        public string CurrentPolish =>
            _words.Length == 0 ? "" : _words[_index].Polish;

        public string ProgressDisplay =>
            _words.Length == 0
                ? ""
                : $"{_index+1}/{_words.Length}";

        // dzięki powyższej RaisePropertyChanged przy UserAnswer,
        // IsAnswerNotEmpty będzie się aktualizować:
        public bool IsAnswerNotEmpty =>
            !string.IsNullOrWhiteSpace(UserAnswer);

        public ReactiveCommand<Unit, Unit> CheckAnswerCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand        { get; }
        public event Action? RequestBack;

        public TestsPageViewModel(string category, string fileName)
        {
            Title = $"Ćwiczenia: {category}";

            // Wczytanie słówek z JSON
            var path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _words   = JsonSerializer.Deserialize<WordPair[]>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                           ?? Array.Empty<WordPair>();
            }
            else
            {
                _words = Array.Empty<WordPair>();
            }

            // Komenda „Sprawdź”
            CheckAnswerCommand = ReactiveCommand.Create(() =>
            {
                if (string.Equals(
                        UserAnswer.Trim(),
                        _words[_index].Finnish,
                        StringComparison.OrdinalIgnoreCase))
                {
                    // poprawna odpowiedź
                    if (_index < _words.Length - 1)
                    {
                        _index++;
                        UserAnswer = ""; // wywoła RaisePropertyChanged dla IsAnswerNotEmpty
                        this.RaisePropertyChanged(nameof(CurrentPolish));
                        this.RaisePropertyChanged(nameof(ProgressDisplay));
                    }
                    else
                    {
                        // ostatnie słowo – wracamy
                        RequestBack?.Invoke();
                    }
                }
                // niepoprawna – możesz dodać feedback, np. kolor TextBox
            });

            // Komenda „Cofnij”
            BackCommand = ReactiveCommand.Create(() => RequestBack?.Invoke());
        }
    }

    public class WordPair
    {
        public string Polish  { get; set; } = "";
        public string Finnish { get; set; } = "";
    }
}
