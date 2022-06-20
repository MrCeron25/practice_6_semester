using Microsoft.Win32;
using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows;
using Project.Views.Pages;

namespace Project.ViewModels
{

    public enum MallAction
    {
        Add,
        Change
    }

    internal class MallPageViewModel : ViewModel
    {
        #region ТЦ
        private Mall _currentMall;
        /// <summary>
        /// ТЦ
        /// </summary>
        public Mall CurrentMall
        {
            get => _currentMall;
            set => Set(ref _currentMall, value);
        }
        #endregion

        #region Действие
        private MallAction? _сurrentMallAction = null;
        /// <summary>
        /// Действие
        /// </summary>
        public MallAction? CurrentMallAction
        {
            get => _сurrentMallAction;
            set => Set(ref _сurrentMallAction, value);
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
        private string _mallStatus;
        /// <summary>
        /// Статус
        /// </summary>
        public string MallStatus
        {
            get => _mallStatus;
            set => Set(ref _mallStatus, value);
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

        #region Команда добавления ТЦ
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
                        where ms.status_name == MallStatus
                        select ms.status_id
                    ).FirstOrDefault();
                    Manager.Instance.Context.Mall.Add(new Mall()
                    {
                        mall_name = CurrentMall.mall_name,
                        status_id = CurrentMall.status_id,
                        cost = CurrentMall.cost,
                        number_of_pavilion = CurrentMall.number_of_pavilion,
                        city = CurrentMall.city,
                        value_added_factor = CurrentMall.value_added_factor,
                        number_of_storeys = CurrentMall.number_of_storeys,
                        photo = CurrentMall.photo,
                    });
                    Manager.Instance.Context.SaveChanges();
                    MessageBox.Show($"Торговый центр добавлен.");
                    Manager.Instance.MainFrameNavigate(new ViewingMallPage());
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

        #region Конструктор
        public MallPageViewModel()
        {
            switch (CurrentMallAction)
            {
                case MallAction.Add:
                    ButtonName = "Добавить";
                    break;
                case MallAction.Change:
                    ButtonName = "Изменить";
                    break;
                default:
                    break;
            }
            ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecute);
            LoadPhotoCommand = new LambdaCommand(OnLoadPhotoCommandExecuted, CanLoadPhotoCommandExecute);
            MallStatuses = new ObservableCollection<string>(
                from ms in Manager.Instance.Context.Mall_statuses
                orderby ms.status_name
                select ms.status_name
                );
            CurrentMall = new Mall();
            //if (CurrentMall == null)
            //{
            //}
            //else
            //{
            //    LoadedImage = Tools.BytesToImage(CurrentMall.photo);
            //}
        }
        #endregion
    }
}
