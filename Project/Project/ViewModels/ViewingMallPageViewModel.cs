using Microsoft.Win32;
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
using System.Windows.Media.Imaging;

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

        public static explicit operator Mall(MallItem mallItem)
        {
            return new Mall
            {
                mall_id = mallItem.mall_id,
                mall_name = mallItem.mall_name,
                status_id = mallItem.status_id,
                cost = mallItem.cost,
                number_of_pavilion = mallItem.number_of_pavilion,
                city = mallItem.city,
                value_added_factor = mallItem.value_added_factor,
                number_of_storeys = mallItem.number_of_storeys,
                photo = mallItem.photo,
            };
        }
        public static explicit operator MallItem(Mall mall)
        {
            return new MallItem
            {
                mall_id = mall.mall_id,
                mall_name = mall.mall_name,
                status_id = mall.status_id,
                status_name = (
                    from m in Manager.Instance.Context.Mall_statuses
                    where m.status_id == mall.status_id
                    select m.status_name
                    ).FirstOrDefault(),
                number_of_pavilion = mall.number_of_pavilion,
                city = mall.city,
                cost = mall.cost,
                number_of_storeys = mall.number_of_storeys,
                value_added_factor = mall.value_added_factor,
                photo = mall.photo
            };
        }
    }

    internal class ViewingMallPageViewModel : ViewModel
    {
        #region ТЦ
        private ObservableCollection<MallItem> _malls;
        /// <summary>
        /// ТЦ
        /// </summary>
        public ObservableCollection<MallItem> Malls
        {
            get => _malls;
            set => Set(ref _malls, value);
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
                IsSelected = value != null;
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
            LoadedImage = null;
            CurrentMall = new Mall();
            CurrentMallAction = MallAction.Add;
            ButtonName = "Добавить";
            Manager.Instance.MainFrameNavigate(new MallPage());
        }
        #endregion

        #region Изменить ТЦ
        public ICommand ChangeMallCommand { get; }
        private bool CanChangeMallCommandExecute(object parameters) => IsSelected;
        private void OnChangeMallCommandExecuted(object parameters)
        {
            CurrentMallAction = MallAction.Change;
            ButtonName = "Изменить";
            CurrentMall = (
                    from m in Manager.Instance.Context.Mall
                    where m.mall_id == SelectedMall.mall_id
                    select m
                    ).FirstOrDefault();
            SelectedMallStatus = SelectedMall.status_name;
            LoadedImage = Tools.BytesToImage(CurrentMall.photo);
            Manager.Instance.MainFrameNavigate(new MallPage());
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
                Malls.Remove(SelectedMall); // или UpdateMalls();
                MessageBox.Show($"Торговый центр удалён.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка :\n{e.Message}");
            }
        }
        #endregion


        #region MallPage

        #region ТЦ
        private Mall _currentMall = new Mall();
        /// <summary>
        /// ТЦ
        /// </summary>
        public Mall CurrentMall
        {
            get => _currentMall;
            set => Set(ref _currentMall, value);
        }
        #endregion


        #region MallAction
        private MallAction _currentMallAction = MallAction.None;
        /// <summary>
        /// MallAction
        /// </summary>
        public MallAction CurrentMallAction
        {
            get => _currentMallAction;
            set => Set(ref _currentMallAction, value);
        }
        #endregion

        #region Статусы
        private ObservableCollection<string> _mallStatuses;
        /// <summary>
        /// Статусы
        /// </summary>
        public ObservableCollection<string> MallStatuses
        {
            get => _mallStatuses;
            set => Set(ref _mallStatuses, value);
        }
        #endregion

        #region Статус
        private string _selectedMallStatus;
        /// <summary>
        /// Статус
        /// </summary>
        public string SelectedMallStatus
        {
            get => _selectedMallStatus;
            set => Set(ref _selectedMallStatus, value);
        }
        #endregion

        #region Img
        private BitmapImage _loadedImage;
        /// <summary>
        /// Img
        /// </summary>
        public BitmapImage LoadedImage
        {
            get => _loadedImage;
            set => Set(ref _loadedImage, value);
        }
        #endregion

        #region ButtonName
        private string _ButtonName;
        /// <summary>
        /// ButtonName
        /// </summary>
        public string ButtonName
        {
            get => _ButtonName;
            set => Set(ref _ButtonName, value);
        }
        #endregion

        #region Команда
        public ICommand ExecuteCommand { get; }
        private bool CanExecuteCommandExecute(object parameters) => true;
        private void OnExecuteCommandExecuted(object parameters)
        {
            if (
                string.IsNullOrEmpty(CurrentMall.city) ||
                string.IsNullOrEmpty(CurrentMall.mall_name) ||
                CurrentMall.photo == null
                )
            {
                MessageBox.Show($"Заполните все поля.");
            }
            else
            {
                try
                {
                    CurrentMall.status_id = (
                        from ms in Manager.Instance.Context.Mall_statuses
                        where ms.status_name == SelectedMallStatus
                        select ms.status_id
                    ).FirstOrDefault();
                    switch (CurrentMallAction)
                    {
                        case MallAction.Add:
                            Manager.Instance.Context.Mall.Add(CurrentMall);
                            MessageBox.Show($"Торговый центр добавлен.");
                            break;
                        case MallAction.Change:
                            MessageBox.Show($"Торговый центр изменён.");
                            break;
                    }
                    Manager.Instance.Context.SaveChanges();
                    Manager.Instance.MainFrameNavigate(new ViewingMallPage());
                    UpdateMalls();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Ошибка :\n{e.Message}");
                }
            }
        }
        #endregion

        #region Загрузка фотографии
        public ICommand LoadPhotoCommand { get; }
        private bool CanLoadPhotoCommandExecute(object parameters) => true;
        private void OnLoadPhotoCommandExecuted(object parameters)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*png;";
            if ((bool)fileDialog.ShowDialog())
            {
                CurrentMall.photo = Tools.GetImageBytes(fileDialog.FileName);
                LoadedImage = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }
        #endregion

        #endregion

        #region UpdateMalls
        private void UpdateMalls()
        {
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

        #region UpdateMallStatuses
        private void UpdateMallStatuses()
        {
            MallStatuses = new ObservableCollection<string>(
                from ms in Manager.Instance.Context.Mall_statuses
                orderby ms.status_name
                select ms.status_name
                );
        }
        #endregion

        #region Конструктор
        public ViewingMallPageViewModel()
        {
            DeleteMallCommand = new LambdaCommand(OnDeleteMallCommandExecuted, CanDeleteMallCommandExecute);
            AddMallCommand = new LambdaCommand(OnAddMallCommandExecuted, CanAddMallCommandExecute);
            ChangeMallCommand = new LambdaCommand(OnChangeMallCommandExecuted, CanChangeMallCommandExecute);
            UpdateMalls();

            ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecute);
            LoadPhotoCommand = new LambdaCommand(OnLoadPhotoCommandExecuted, CanLoadPhotoCommandExecute);
            UpdateMallStatuses();
        }
        #endregion
    }
}
