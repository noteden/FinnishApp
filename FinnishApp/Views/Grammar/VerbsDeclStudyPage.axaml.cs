using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FinnishApp.Views.Grammar;

public partial class VerbsDeclStudyPage : UserControl
{
    public VerbsDeclStudyPage()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent() 
        => AvaloniaXamlLoader.Load(this);
}