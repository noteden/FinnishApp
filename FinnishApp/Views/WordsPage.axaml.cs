using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FinnishApp.ViewModels;

namespace FinnishApp.Views
{
    public partial class WordsPage : UserControl
    {
        public WordsPageViewModel ViewModel
        {
            get => (WordsPageViewModel)DataContext!;
            set => DataContext = value;
        }

        public WordsPage()
        {
            InitializeComponent();
            ViewModel = new WordsPageViewModel();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}