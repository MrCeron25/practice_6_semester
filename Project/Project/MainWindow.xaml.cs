using System.Windows;

namespace Project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = Frame;
            Manager.MainFrame.Navigate(new LoginPage());
        }
    }
}
