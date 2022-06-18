using System.Windows.Controls;

namespace Project
{
    public class Manager
    {
        private static Manager _instance;
        public static Manager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Manager();
                }
                return _instance;
            }
        }

        public Entities Context { get; set; }
        public Frame MainFrame { get; set; }
        public Frame MenuFrame { get; set; }
        public void MenuFrameNavigate(object content)
        {
            if (Instance.MenuFrame.CanGoBack)
            {
                Instance.MenuFrame.NavigationService.RemoveBackEntry();
            }
            Instance.MenuFrame.Navigate(content);
        }

        public void MainFrameNavigate(object content)
        {
            if (Instance.MainFrame.CanGoBack)
            {
                Instance.MainFrame.NavigationService.RemoveBackEntry();
            }
            Instance.MainFrame.Navigate(content);
        }
    }
}
