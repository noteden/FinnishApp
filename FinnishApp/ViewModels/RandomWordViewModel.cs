using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Reactive;
using ReactiveUI;
using FinnishApp.Models;

namespace FinnishApp.ViewModels;

public class RandomWordViewModel: ReactiveObject
{
    private readonly string _folderPath = "Data";

        private RandomWord _currentWord;
        private string _correctAnswer;

        private string _polishWord;
        public string PolishWord
        {
            get => _polishWord;
            set => this.RaiseAndSetIfChanged(ref _polishWord, value);
        }

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set => this.RaiseAndSetIfChanged(ref _userInput, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public ReactiveCommand<Unit, Unit> StartQuizCommand { get; }
        public ReactiveCommand<Unit, Unit> CheckAnswerCommand { get; }

        public RandomWordViewModel()
        {
            StartQuizCommand = ReactiveCommand.Create(StartQuiz);
            CheckAnswerCommand = ReactiveCommand.Create(CheckAnswer);
        }

        private void StartQuiz()
        {
            try
            {
                var jsonFiles = Directory.GetFiles(_folderPath, "*.json");
                if (!jsonFiles.Any())
                {
                    Message = "Brak plików JSON w folderze.";
                    return;
                }

                var random = new Random();
                var randomFile = jsonFiles[random.Next(jsonFiles.Length)];
                var content = File.ReadAllText(randomFile);
                var words = JsonSerializer.Deserialize<List<RandomWord>>(content);

                if (words == null || words.Count == 0)
                {
                    Message = "Plik JSON jest pusty.";
                    return;
                }

                _currentWord = words[random.Next(words.Count)];

                bool isPlural = random.Next(2) == 0;
                var inflections = isPlural ? _currentWord.InflectionsPlural : _currentWord.InflectionsSingular;

                var nonEmptyInflections = inflections
                    .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                    .ToList();

                if (!nonEmptyInflections.Any())
                {
                    Message = "Brak odmian dla tego słowa.";
                    return;
                }

                var selectedInflection = nonEmptyInflections[random.Next(nonEmptyInflections.Count)];
                _correctAnswer = selectedInflection.Value;

                PolishWord = $"{_currentWord.Polish} ({selectedInflection.Key.TrimEnd(':')})";
                UserInput = "";
                Message = "Podaj odpowiednią formę po fińsku:";
            }
            catch (Exception ex)
            {
                Message = "Błąd: " + ex.Message;
            }
        }

        private void CheckAnswer()
        {
            if (string.IsNullOrWhiteSpace(UserInput))
            {
                Message = "Nie podałeś odpowiedzi.";
                return;
            }

            if (UserInput.Trim().Equals(_correctAnswer, StringComparison.OrdinalIgnoreCase))
            {
                Message = "Poprawnie!";
            }
            else
            {
                Message = $"Błąd. Poprawna odpowiedź: {_correctAnswer}";
            }
        }
}