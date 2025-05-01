using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reactive;

namespace FinnishApp.ViewModels
{
    public class WordsPageViewModel : ReactiveObject
    {
        // teraz odwołujemy się do top–level Level
        public ReactiveCommand<Level, Unit>    SelectLevelCommand    { get; }
        public ReactiveCommand<Category, Unit> SelectCategoryCommand { get; }
        public ReactiveCommand<Unit, Unit>     CheckAnswerCommand    { get; }

        public ObservableCollection<Category> Categories { get; }
        
        private bool _isSelecting = true;
        public bool IsSelecting
        {
            get => _isSelecting;
            private set => this.RaiseAndSetIfChanged(ref _isSelecting, value);
        }

        private bool _isInQuiz = false;
        public bool IsInQuiz
        {
            get => _isInQuiz;
            private set => this.RaiseAndSetIfChanged(ref _isInQuiz, value);
        }

        private string _currentPolish = string.Empty;
        public string CurrentPolishWord
        {
            get => _currentPolish;
            private set => this.RaiseAndSetIfChanged(ref _currentPolish, value);
        }

        private string _userAnswer = string.Empty;
        public string UserAnswer
        {
            get => _userAnswer;
            set
            {
                this.RaiseAndSetIfChanged(ref _userAnswer, value);
                this.RaisePropertyChanged(nameof(IsAnswerNotEmpty));
            }
        }

        public bool IsAnswerNotEmpty => !string.IsNullOrWhiteSpace(UserAnswer);
        private int _totalCount   = 0;
        private int _correctCount = 0;
        public string ProgressDisplay => $"{_correctCount}/{_totalCount}";
        
        private readonly Dictionary<Level, List<Category>> _allCategories = new();
        private Queue<WordPair> _quizQueue = new();

        public WordsPageViewModel()
        {
            Categories            = new ObservableCollection<Category>();
            SelectLevelCommand    = ReactiveCommand.Create<Level>(SelectLevel);
            SelectCategoryCommand = ReactiveCommand.Create<Category>(StartQuiz);
            CheckAnswerCommand    = ReactiveCommand.Create(CheckAnswer);
            
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var text    = await File.ReadAllTextAsync("Data/Words.json");
            var opts    = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var raw     = JsonSerializer.Deserialize<Dictionary<string, List<Category>>>(text, opts)
                          ?? new();

            foreach (var kv in raw)
                if (Enum.TryParse<Level>(kv.Key, true, out var lvl))
                    _allCategories[lvl] = kv.Value;
        }

        private void SelectLevel(Level level)
        {
            Categories.Clear();
            if (_allCategories.TryGetValue(level, out var list))
                foreach (var cat in list)
                    Categories.Add(cat);
        }

        private void StartQuiz(Category category)
        {
            _quizQueue    = new Queue<WordPair>(category.Words);
            _totalCount   = _quizQueue.Count;
            _correctCount = 0;
            IsSelecting   = false;
            IsInQuiz      = true;
            NextWord();
        }

        private void NextWord()
        {
            if (_quizQueue.Count == 0)
            {
                IsInQuiz    = false;
                IsSelecting = true;
                return;
            }

            var wp = _quizQueue.Dequeue();
            CurrentPolishWord = wp.Polish;
            UserAnswer        = string.Empty;
            _currentFinnish   = wp.Finnish;
        }

        private string _currentFinnish = string.Empty;
        private void CheckAnswer()
        {
            if (string.Equals(UserAnswer?.Trim(),
                              _currentFinnish,
                              StringComparison.OrdinalIgnoreCase))
                _correctCount++;

            NextWord();
            this.RaisePropertyChanged(nameof(ProgressDisplay));
        }
    }
    
    public class Category
    {
        public string Name { get; set; } = string.Empty;
        public List<WordPair> Words { get; set; } = new();
    }

    public class WordPair
    {
        public string Polish  { get; set; } = string.Empty;
        public string Finnish { get; set; } = string.Empty;
    }
}
