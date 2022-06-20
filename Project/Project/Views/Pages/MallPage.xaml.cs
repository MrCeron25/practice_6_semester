using Project.Models;
using Project.ViewModels;
using System.Windows.Controls;

namespace Project.Views.Pages
{
    public partial class MallPage : Page
    {
        public MallPage(MallAction mallAction, Mall mall = null)
        {
            InitializeComponent();
            ((MallPageViewModel)DataContext).CurrentMallAction = mallAction;
            ((MallPageViewModel)DataContext).CurrentMall = mall;
        }
    }
}
