using Project.Models;
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

        public practiceEntities Context { get; set; }
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

        /// <summary>
        /// По пользователю загружает нужный интерфейс
        /// </summary>
        /// <param name="employee"></param>
        public void LoadEmployeeInterface(Employees employee)
        {
            if (employee != null)
            {
                Page main = ManagerRoles.ManagerPages[employee.role][0];
                if (main != null)
                {
                    Instance.MainFrameNavigate(main);
                }
                Page menu = ManagerRoles.ManagerPages[employee.role][1];
                if (menu != null)
                {
                    Instance.MenuFrameNavigate(menu);
                }
            }
        }
    }
}
