using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace FinnishApp.Views
{
    public partial class ExercisesPage : UserControl
    {
        public event Action? BackClicked;
        public event Action? FlashcardsClicked;
        public event Action? TestsClicked;

        public ExercisesPage()
        {
            InitializeComponent();

            this.FindControl<Button>("BackButton")
                .Click += (_, __) => BackClicked?.Invoke();

            this.FindControl<Button>("FlashcardsButton")
                .Click += (_, __) => FlashcardsClicked?.Invoke();

            this.FindControl<Button>("TestsButton")
                .Click += (_, __) => TestsClicked?.Invoke();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}