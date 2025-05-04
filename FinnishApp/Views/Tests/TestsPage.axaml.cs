using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace FinnishApp.Views
{
    public partial class TestsPage : UserControl
    {
        public event Action? BackClicked;
        public event Action? BeginnerClicked;
        public event Action? IntermediateClicked;
        public event Action? AdvancedClicked;
        
        
        public TestsPage()
        {
            InitializeComponent();

            this.FindControl<Button>("BackButton")
                .Click += (_, __) => BackClicked?.Invoke();

            this.FindControl<Button>("BeginnerButton")
                .Click += (_, __) => BeginnerClicked?.Invoke();

            this.FindControl<Button>("IntermediateButton")
                .Click += (_, __) => IntermediateClicked?.Invoke();

            this.FindControl<Button>("AdvancedButton")
                .Click += (_, __) => AdvancedClicked?.Invoke();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}