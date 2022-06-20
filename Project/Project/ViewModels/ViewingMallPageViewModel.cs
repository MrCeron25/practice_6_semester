using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class MallItem
    {
        public long mall_id { get; set; }
        public string mall_name { get; set; }
        public long status_id { get; set; }
        public string status_name { get; set; }
        public int number_of_pavilion { get; set; }
        public string city { get; set; }
        public decimal cost { get; set; }
        public int number_of_storeys { get; set; }
        public double value_added_factor { get; set; }
        public byte[] photo { get; set; }
    }

    internal class ViewingMallPageViewModel : ViewModel
    {
        #region ТЦ
        private ObservableCollection<MallItem> _mallsName;
        /// <summary>
        /// ТЦ
        /// </summary>
        public ObservableCollection<MallItem> Malls
        {
            get => _mallsName;
            set => Set(ref _mallsName, value);
        }
        #endregion

        #region Выбранный ТЦ
        private MallItem _selectedMall;
        /// <summary>
        /// Выбранный ТЦ
        /// </summary>
        public MallItem SelectedMall
        {
            get => _selectedMall;
            set
            {
                if (value != null)
                {
                    IsSelected = true;
                }
                Set(ref _selectedMall, value);
            }
        }
        #endregion

        #region ТЦ выбран
        private bool _isSelected = false;
        /// <summary>
        /// ТЦ выбран
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
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

        #region Добавить ТЦ
        public ICommand AddMallCommand { get; }
        private bool CanAddMallCommandExecute(object parameters) => true;
        private void OnAddMallCommandExecuted(object parameters)
        {
            Manager.Instance.MainFrameNavigate(new MallPage(MallAction.Add));
        }
        #endregion

        #region Изменить ТЦ
        public ICommand ChangeMallCommand { get; }
        private bool CanChangeMallCommandExecute(object parameters) => IsSelected;
        private void OnChangeMallCommandExecuted(object parameters)
        {
            Mall mall = (
                    from m in Manager.Instance.Context.Mall
                    where m.mall_id == SelectedMall.mall_id
                    select m
                    ).FirstOrDefault();
            Manager.Instance.MainFrameNavigate(new MallPage(MallAction.Change, mall));
        }
        #endregion

        #region Удалить ТЦ
        public ICommand DeleteMallCommand { get; }
        private bool CanDeleteMallCommandExecute(object parameters) => IsSelected;
        private void OnDeleteMallCommandExecuted(object parameters)
        {
            try
            {
                Mall mall = (
                    from m in Manager.Instance.Context.Mall
                    where m.mall_id == SelectedMall.mall_id
                    select m
                    ).FirstOrDefault();
                Manager.Instance.Context.Mall.Remove(mall);
                Manager.Instance.Context.SaveChanges();
                Malls.Remove(SelectedMall);
                SelectedMall = null;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка :\n{e.Message}");
            }
        }
        #endregion


        #region Конструктор
        public ViewingMallPageViewModel()
        {
            DeleteMallCommand = new LambdaCommand(OnDeleteMallCommandExecuted, CanDeleteMallCommandExecute);
            AddMallCommand = new LambdaCommand(OnAddMallCommandExecuted, CanAddMallCommandExecute);
            ChangeMallCommand = new LambdaCommand(OnChangeMallCommandExecuted, CanChangeMallCommandExecute);
            Malls = new ObservableCollection<MallItem>(
                        from mall in Manager.Instance.Context.Mall
                        join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                        orderby mall.city, ms.status_name
                        select new MallItem
                        {
                            mall_id = mall.mall_id,
                            mall_name = mall.mall_name,
                            status_name = ms.status_name,
                            status_id = ms.status_id,
                            number_of_pavilion = mall.number_of_pavilion,
                            city = mall.city,
                            cost = mall.cost,
                            number_of_storeys = mall.number_of_storeys,
                            value_added_factor = mall.value_added_factor,
                            photo = mall.photo
                        });
        }
        #endregion
    }
}
