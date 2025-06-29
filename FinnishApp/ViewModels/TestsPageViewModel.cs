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
        
        
        private string _userAnswer = "";
        public string UserAnswer
        {
            get => _userAnswer;
            set
            {
                this.RaiseAndSetIfChanged(ref _userAnswer, value);
                this.RaisePropertyChanged(nameof(IsAnswerNotEmpty));
                
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = "";
                }
            }
        }
        
        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                this.RaiseAndSetIfChanged(ref _errorMessage, value);
                this.RaisePropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public string CurrentPolish =>
            _words.Length == 0 ? "" : _words[_index].Polish;

        public string ProgressDisplay =>
            _words.Length == 0
                ? ""
                : $"{_index+1}/{_words.Length}";
        
        public bool IsAnswerNotEmpty =>
            !string.IsNullOrWhiteSpace(UserAnswer);

        public ReactiveCommand<Unit, Unit> CheckAnswerCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand        { get; }
        public event Action? RequestBack;

        public TestsPageViewModel(string category, string fileName)
        {
            string cleaned = category.TrimStart(' ', '-');
            Title = $"Ćwiczenia: {cleaned}";
            
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


            CheckAnswerCommand = ReactiveCommand.Create(() =>
            {
                var correct = string.Equals(
                    UserAnswer.Trim(),
                    _words[_index].Finnish,
                    StringComparison.OrdinalIgnoreCase);
                {
                    if (correct)
                    {
                        ErrorMessage = "";
                        if (_index < _words.Length - 1)
                        {
                            _index++;
                            UserAnswer = "";
                            this.RaisePropertyChanged(nameof(CurrentPolish));
                            this.RaisePropertyChanged(nameof(ProgressDisplay));
                        }
                        else
                        {
                            RequestBack?.Invoke();
                        }
                    }
                    else
                    {
                        ErrorMessage = "Niepoprawna odpowiedź – spróbuj ponownie.";
                    }
                }
            });
            
            BackCommand = ReactiveCommand.Create(() => RequestBack?.Invoke());
        }
    }
}
