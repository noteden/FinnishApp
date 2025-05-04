using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using FinnishApp.ViewModels;
using FinnishApp.Views;
using FinnishApp.Views.Tests;

namespace FinnishApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Wait until the window is actually open before swapping pages:
            this.Opened += (_, __) => ShowPage(new MainMenuPage());
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void ShowPage(UserControl page)
        {
            // Instead of FindControl, cast the Window.Content to Grid directly:
            if (this.Content is not Grid rootGrid)
                throw new InvalidOperationException("MainWindow.Content is not a Grid!");

            rootGrid.Children.Clear();
            rootGrid.Children.Add(page);

            switch (page)
            {
                case MainMenuPage m:
                    m.LearnClicked     += () => ShowPage(new LearningPage());
                    m.ExercisesClicked += () => ShowPage(new ExercisesPage());
                    m.ExitClicked      += () => Close();
                    break;

                case LearningPage lp:
                    lp.VocabularyClicked    += () => ShowPage(new VocabularyPage());
                    lp.BackClicked     += () => ShowPage(new MainMenuPage());
                    break;

                case ExercisesPage ep:
                    ep.FlashcardsClicked    += () => ShowPage(new FlashcardsPage());
                    ep.TestsClicked      += () => ShowPage(new TestsPage());
                    ep.BackClicked  += () => ShowPage(new MainMenuPage());
                    break;
                
                case VocabularyPage np:
                    np.BackClicked    += ()             => ShowPage(new LearningPage());
                    np.BeginnerClicked+= ()             => {
                        var page = new BeginnerCategoriesPage();
                        page.BackClicked        += () => ShowPage(new VocabularyPage());
                        ShowPage(page);
                    };
                    break;
                
                case FlashcardsPage fp:
                    fp.BackClicked    += () => ShowPage(new ExercisesPage());
                    fp.BeginnerClicked += () =>
                    {
                        var page = new FlashcardsBeginnerCategoriesPage();
                        page.BackClicked += () => ShowPage(new FlashcardsPage());
                        ShowPage(page);
                    };
                    break;
                
                case TestsPage tp:
                    tp.BackClicked    += () => ShowPage(new ExercisesPage());
                    tp.BeginnerClicked += () =>
                    {
                        var page = new TestsBeginnerCategoriesPage();
                        page.BackClicked    += () => ShowPage(new TestsPage());
                        ShowPage(page);
                    };
                    break;
                
                case TestsBeginnerCategoriesPage tbcp:
                    tbcp.BackClicked     += () => ShowPage(new FlashcardsPage());
                    tbcp.CategoryClicked += name =>
                    {
                        // name to np. "Kolory"
                        var vm = new TestsPageViewModel(
                            category: name,
                            fileName: $"{name}.json"
                        );
                        vm.RequestBack += () => ShowPage(tbcp);

                        var tr = new TestsStudyPage() { DataContext = vm };
                        ShowPage(tr);
                    };
                    break;
                
                case BeginnerCategoriesPage bp:
                    bp.CategoryClicked += name =>
                    {
                        var vm = new StudyPageViewModel();
                        vm.RequestBack += () => ShowPage(bp);
                        vm.LoadCategory(name);

                        var sp = new StudyPage { DataContext = vm };
                        ShowPage(sp);
                    };
                    bp.BackClicked += () => ShowPage(new VocabularyPage());
                    break;
                
                case FlashcardsBeginnerCategoriesPage fbcp:
                    fbcp.BackClicked += () => ShowPage(new FlashcardsPage());
                    fbcp.CategoryClicked += categoryName =>
                    {
                        // assume your JSON is "Data/{categoryName}.json"
                        var vm = new FlashcardsStudyViewModel(
                            categoryName,
                            $"{categoryName}.json"
                        );
                        vm.RequestBack += () => ShowPage(fbcp);
                        var study = new FlashcardsStudyPage { DataContext = vm };
                        ShowPage(study);
                    };
                    break;

            }
        }
    }
}