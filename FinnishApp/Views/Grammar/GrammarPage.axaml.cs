using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace FinnishApp.Views
{
    public partial class GrammarPage : UserControl
    {
        public event Action? BackClicked;
        public event Action? VerbsClicked;
        public event Action? NounsClicked;
        public event Action? SentConClicked;

        public GrammarPage()
        {
            InitializeComponent();

            this.FindControl<Button>("BackButton")
                .Click += (_,__) => BackClicked?.Invoke();

            this.FindControl<Button>("VerbsDeclButton")
                .Click += (_,__) => VerbsClicked?.Invoke();

            this.FindControl<Button>("NounsDeclButton")
                .Click += (_,__) => NounsClicked?.Invoke();

            this.FindControl<Button>("SentConButton")
                .Click += (_,__) => SentConClicked?.Invoke();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}