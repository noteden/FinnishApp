using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;

namespace FinnishApp.Views
{
    public partial class MainMenuPage : UserControl
    {
        public event Action? LearnClicked;
        public event Action? ExercisesClicked;
        public event Action? ExitClicked;

        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnLearnClicked(object? sender, RoutedEventArgs e)
            => LearnClicked?.Invoke();

        private void OnExercisesClicked(object? sender, RoutedEventArgs e)
            => ExercisesClicked?.Invoke();

        private void OnExitClicked(object? sender, RoutedEventArgs e)
            => ExitClicked?.Invoke();
    }
}