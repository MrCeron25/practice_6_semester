using Project.Infrastructure.Commands;
using Project.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class MallItem
    {
        public long mall_id { get; set; }
        public string mall_name { get; set; }
        public string status_name { get; set; }
        public int number_of_pavilion { get; set; }
        public string city { get; set; }
        public decimal cost { get; set; }
        public int number_of_storeys { get; set; }
        public double value_added_factor { get; set; }
        public byte[] photo { get; set; }
    }

    internal class MallPageViewModel : ViewModel
    {
        #region ТЦ
        private List<MallItem> _mallsName;
        /// <summary>
        /// ТЦ
        /// </summary>
        public List<MallItem> Malls
        {
            get => _mallsName;
            set => Set(ref _mallsName, value);
        }
        #endregion

        #region ТЦ
        private string _mallTitleName = "Торговые центры";
        /// <summary>
        /// ТЦ
        /// </summary>
        public string MallTitleName
        {
            get => _mallTitleName;
            set => Set(ref _mallTitleName, value);
        }
        #endregion

        #region Команда перехода на страницу ТЦ
        public ICommand GoMallCommand { get; }
        private bool CanGoMallCommandExecute(object parameters) => true;
        private void OnGoMallCommandExecuted(object parameters)
        {

        }

        #endregion

        #region Команда перехода на страницу павильонов
        public ICommand GoPavilionCommand { get; }
        private bool CanGoPavilionCommandExecute(object parameters) => true;
        private void OnGoPavilionCommandExecuted(object parameters)
        {

        }
        #endregion

        #region Команда выхода
        public ICommand ExitCommand { get; }
        private bool CanExitCommandExecute(object p) => true;
        private void OnExitCommandExecuted(object p)
        {

        }
        #endregion

        #region Конструктор
        public MallPageViewModel()
        {
            GoMallCommand = new LambdaCommand(OnGoMallCommandExecuted, CanGoMallCommandExecute);
            GoPavilionCommand = new LambdaCommand(OnGoPavilionCommandExecuted, CanGoPavilionCommandExecute);
            ExitCommand = new LambdaCommand(OnExitCommandExecuted, CanExitCommandExecute);
            Malls = (from mall in Manager.Instance.Context.Mall
                     join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                     select new MallItem
                     {
                         mall_id = mall.mall_id,
                         mall_name = mall.mall_name,
                         status_name = ms.status_name,
                         number_of_pavilion = mall.number_of_pavilion,
                         city = mall.city,
                         cost = mall.cost,
                         number_of_storeys = mall.number_of_storeys,
                         value_added_factor = mall.value_added_factor,
                         photo = mall.photo
                     }).ToList();
        }
        #endregion
    }
}
