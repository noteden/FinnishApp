using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;

namespace FinnishApp.Views
{
    public partial class LearningPage : UserControl
    {
        public event Action? BackClicked;
        public event Action? VocabularyClicked;
        public event Action? GrammarClicked; 

        public LearningPage()
        {
            InitializeComponent();
            
            this.FindControl<Button>("BackButton")
                .Click += (_, __) => BackClicked?.Invoke();

            this.FindControl<Button>("VocabularyButton")
                .Click += (_, __) => VocabularyClicked?.Invoke();
            
            this.FindControl<Button>("GrammarButton")
                .Click += (_,__) => GrammarClicked?.Invoke();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}