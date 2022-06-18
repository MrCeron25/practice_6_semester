using System.Collections.Generic;
using System.Windows.Controls;

namespace Project
{
    public class ManagerRoles
    {
        public static Dictionary<long, Page> ManagerPages = new Dictionary<long, Page>()
        {
            { 1, null }, // Администратор
            { 2, null }, // Менеджер А
            { 3, new ManagerCPage() }, // Менеджер С
            { 4, null } // Удален
        };
    }
}
