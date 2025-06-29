using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FinnishApp.ViewModels;
using FinnishApp.Models;

namespace FinnishApp.Views
{
    public partial class StudyPage : UserControl
    {
        public StudyPage()
        {
            InitializeComponent();
        }
        
        private void OnCellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
        {
            // klik w kolumnę „Fiński”?
            if (e.Column.Header?.ToString() == "Fiński" &&
                e.Row.DataContext is WordPair pair)
            {
                // otwórz modalne okienko
                var infWin = new InflectionsWindow(pair);
                // Rodzic to Twoje okno główne:
                var main = (Window) this.VisualRoot!;
                infWin.ShowDialog(main);
            }
        }

        private void InitializeComponent() 
            => AvaloniaXamlLoader.Load(this);
    }
}