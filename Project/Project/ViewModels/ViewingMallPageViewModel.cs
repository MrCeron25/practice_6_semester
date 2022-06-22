using Microsoft.Win32;
using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Project.ViewModels
{
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

        #region ТЦ название
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
            CurrentActionEntities = ActionEntities.Add;
            ButtonName = "Добавить";
            Manager.Instance.MainFrameNavigate(new MallPage());
        }
        #endregion

        #region Изменить ТЦ
        public ICommand ChangeMallCommand { get; }
        private bool CanChangeMallCommandExecute(object parameters) => IsSelected;
        private void OnChangeMallCommandExecuted(object parameters)
        {
            CurrentActionEntities = ActionEntities.Change;
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
                mall.status_id = (
                    from m in Manager.Instance.Context.Mall_statuses
                    where m.status_name == DeleteNameSorting
                    select m.status_id
                    ).FirstOrDefault();
                Manager.Instance.Context.SaveChanges();
                Malls.Remove(SelectedMall); // или UpdateMalls();
                MessageBox.Show($"Торговый центр удалён.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка :\n{e}");
            }
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

        #region Текущий ТЦ
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

        #region ActionEntities
        private ActionEntities _currentActionEntities = ActionEntities.None;
        /// <summary>
        /// ActionEntities
        /// </summary>
        public ActionEntities CurrentActionEntities
        {
            get => _currentActionEntities;
            set => Set(ref _currentActionEntities, value);
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
            if (CurrentMall.cost < 0 ||
                CurrentMall.number_of_pavilion < 0 ||
                CurrentMall.value_added_factor < 0 ||
                CurrentMall.number_of_storeys < 0)
            {
                MessageBox.Show($"Числовые поля должны быть положительными.");
            }
            else if (string.IsNullOrEmpty(CurrentMall.mall_name.Trim()) ||
                     SelectedMallStatus == null ||
                     string.IsNullOrEmpty(CurrentMall.city.Trim()) ||
                     CurrentMall.photo == null)
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
                    CurrentMall.mall_name = CurrentMall.mall_name.Trim();
                    CurrentMall.city = CurrentMall.city.Trim();
                    switch (CurrentActionEntities)
                    {
                        case ActionEntities.Add:
                            Manager.Instance.Context.Mall.Add(CurrentMall);
                            MessageBox.Show($"Торговый центр добавлен.");
                            break;
                        case ActionEntities.Change:
                            MessageBox.Show($"Торговый центр изменён.");
                            break;
                    }
                    Manager.Instance.Context.SaveChanges();
                    Manager.Instance.MainFrameNavigate(new ViewingMallPage());
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Ошибка :\n{e}");
                }
            }
            SelectedMallStatusSorting = AllNameSorting;
            UpdateMalls();
        }
        #endregion

        #region Загрузка фотографии
        public ICommand LoadPhotoCommand { get; }
        private bool CanLoadPhotoCommandExecute(object parameters) => true;
        private void OnLoadPhotoCommandExecuted(object parameters)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*png;"
                };
                if ((bool)fileDialog.ShowDialog())
                {
                    if (fileDialog.FileName.EndsWith(".jpg") ||
                        fileDialog.FileName.EndsWith(".png"))
                    {
                        CurrentMall.photo = Tools.GetImageBytes(fileDialog.FileName);
                        LoadedImage = new BitmapImage(new Uri(fileDialog.FileName));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region Сортировка статусов торговых центров
        private ObservableCollection<string> _mallStatusesSorting = GetMallStatuses();
        /// <summary>
        /// Сортировка статусов торговых центров
        /// </summary>
        public ObservableCollection<string> MallStatusesSorting
        {
            get => _mallStatusesSorting;
            set => Set(ref _mallStatusesSorting, value);
        }
        #endregion

        #region Выбранная сортировка статусов
        private string _selectedMallStatusSorting;
        /// <summary>
        /// Выбранная сортировка статусов
        /// </summary>
        public string SelectedMallStatusSorting
        {
            get => _selectedMallStatusSorting;
            set
            {
                UpdateCities();
                SelectedCity = null;
                CityIsEnabled = value != null;
                if (AllNameSorting == value)
                {
                    UpdateMalls();
                }
                else
                {
                    Malls = new ObservableCollection<MallItem>(
                    from mall in Manager.Instance.Context.Mall
                    join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                    orderby mall.city, ms.status_name
                    where ms.status_name == value
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
                Set(ref _selectedMallStatusSorting, value);
            }
        }
        #endregion

        #region Города
        private ObservableCollection<string> _cities;
        /// <summary>
        /// Города
        /// </summary>
        public ObservableCollection<string> Cities
        {
            get => _cities;
            set => Set(ref _cities, value);
        }
        #endregion

        #region Состояние городов
        private bool _сityIsEnabled;
        /// <summary>
        /// Состояние городов
        /// </summary>
        public bool CityIsEnabled
        {
            get => Cities.Count != 0;
            set => Set(ref _сityIsEnabled, value);
        }
        #endregion

        #region Выбранный город
        private string _selectedCity;
        /// <summary>
        /// Выбранный город
        /// </summary>
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (value != null)
                {
                    if (AllNameSorting == SelectedMallStatusSorting)
                    {
                        Malls = new ObservableCollection<MallItem>(
                        from mall in Manager.Instance.Context.Mall
                        join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                        orderby mall.city, ms.status_name
                        where mall.city == value &&
                              ms.status_name != DeleteNameSorting
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
                    else
                    {
                        Malls = new ObservableCollection<MallItem>(
                        from mall in Manager.Instance.Context.Mall
                        join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                        orderby mall.city, ms.status_name
                        where mall.city == value &&
                              ms.status_name == SelectedMallStatusSorting
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
                }
                Set(ref _selectedCity, value);
            }
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

        #region UpdateMalls
        private void UpdateMalls()
        {
            Malls = new ObservableCollection<MallItem>(
            from mall in Manager.Instance.Context.Mall
            join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
            orderby mall.city, ms.status_name
            where ms.status_name != DeleteNameSorting
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

        #region GetMallStatuses
        private static ObservableCollection<string> GetMallStatuses()
        {
            return new ObservableCollection<string>(
                from ms in Manager.Instance.Context.Mall_statuses
                orderby ms.status_name
                select ms.status_name
                );
        }
        #endregion

        #region GetCities
        private void UpdateCities()
        {
            Cities = GetCities();
        }
        #endregion

        #region GetCities
        private ObservableCollection<string> GetCities()
        {
            return new ObservableCollection<string>((
                    from m in Manager.Instance.Context.Mall
                    orderby m.city
                    where m.status_id != (from ms in Manager.Instance.Context.Mall_statuses
                                          where ms.status_name == DeleteNameSorting
                                          select ms.status_id).FirstOrDefault()
                    select m.city
                ).Distinct().ToList());
        }
        #endregion

        #region UpdateMallStatuses
        private void UpdateMallStatuses()
        {
            MallStatuses = GetMallStatuses();
        }
        #endregion

        #region Статус сортировки Всё
        private string _allNameSorting = "Всё";
        /// <summary>
        /// Статус сортировки Всё
        /// </summary>
        public string AllNameSorting
        {
            get => _allNameSorting;
            set => Set(ref _allNameSorting, value);
        }
        #endregion

        #region Статус сортировки Удалён
        private string _deleteNameSorting = "Удалён";
        /// <summary>
        /// Статус сортировки Удалён
        /// </summary>
        public string DeleteNameSorting
        {
            get => _deleteNameSorting;
            set => Set(ref _deleteNameSorting, value);
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

            // убираю сортировку по удаленным ТЦ
            MallStatusesSorting.Remove(DeleteNameSorting);
            // добавляю общую сортировку
            MallStatusesSorting.Add(AllNameSorting);
            // текущим выбранным ставим все
            SelectedMallStatusSorting = AllNameSorting;

            UpdateCities();
        }
        #endregion
    }
}