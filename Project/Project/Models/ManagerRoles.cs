using Project.Views.Pages;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Project
{
    public class ManagerRoles
    {
        public static Dictionary<long, List<Page>> ManagerPages = new Dictionary<long, List<Page>>()
        {
            { 1, null }, // Администратор
            { 2, null }, // Менеджер А
            { 3, new List<Page>() { new MallPage(), new ManagerCMenuPage() } }, // Менеджер С
            { 4, null } // Удален
        };
    }
}
