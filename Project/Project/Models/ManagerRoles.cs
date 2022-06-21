using Project.Views.Pages;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Project
{
    public class ManagerRoles
    {
        /// <summary>
        /// Менеджер по загрузке страниц для каждого пользователя
        /// </summary>
        public static Dictionary<long, List<Page>> ManagerPages = new Dictionary<long, List<Page>>()
        {
            { 0, new List<Page>() { new LoginPage(), new MainMenuPage() } }, // Гость (форма входа)
            { 1, null }, // Администратор
            { 2, null }, // Менеджер А
            { 3, new List<Page>() { new ViewingMallPage(), new ManagerCMenuPage() } }, // Менеджер С
            { 4, null } // Удален
        };
    }
}
