using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FinnishApp.Views
{
    public partial class TestsStudyPage : UserControl
    {
        public TestsStudyPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent() 
            => AvaloniaXamlLoader.Load(this);
    }
}