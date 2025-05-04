using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FinnishApp.Views
{
    public partial class StudyPage : UserControl
    {
        public StudyPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent() 
            => AvaloniaXamlLoader.Load(this);
    }
}