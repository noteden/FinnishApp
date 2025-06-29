using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FinnishApp.ViewModels;
using FinnishApp.Models;

namespace FinnishApp.Views
{
    public partial class InflectionsWindow : Window
    {
        public InflectionsWindow(WordPair pair)
        {
            InitializeComponent();
            DataContext = new InflectionsWindowViewModel(pair);
        }

        private void InitializeComponent() 
            => AvaloniaXamlLoader.Load(this);

        private void OnCloseClicked(object? sender, RoutedEventArgs e)
            => Close();
    }
}