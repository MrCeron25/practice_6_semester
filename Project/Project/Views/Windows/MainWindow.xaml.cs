using Project.Models;
using Project.Views.Pages;
using System.Windows;

namespace Project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.Instance.Context = new practiceEntities();

            Manager.Instance.MainFrame = MainFrame;
            Manager.Instance.MenuFrame = MenuFrame;

            Manager.Instance.MainFrameNavigate(new LoginPage());
            Manager.Instance.MenuFrameNavigate(new MainMenuPage());
        }
    }
}
