using Microsoft.Win32;
using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
                IsSelectedMall = value != null;
                Set(ref _selectedMall, value);
            }
        }
        #endregion

        #region ТЦ выбран
        private bool _isSelectedMall = false;
        /// <summary>
        /// ТЦ выбран
        /// </summary>
        public bool IsSelectedMall
        {
            get => _isSelectedMall;
            set => Set(ref _isSelectedMall, value);
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
            CurrentMallActionEntities = ActionEntities.Add;
            Manager.Instance.MainFrameNavigate(new MallPage());
        }
        #endregion

        #region Изменить ТЦ
        public ICommand ChangeMallCommand { get; }
        private bool CanChangeMallCommandExecute(object parameters) => IsSelectedMall;
        private void OnChangeMallCommandExecuted(object parameters)
        {
            CurrentMallActionEntities = ActionEntities.Change;
            Manager.Instance.MainFrameNavigate(new MallPage());
        }
        #endregion

        #region Удалить ТЦ
        public ICommand DeleteMallCommand { get; }
        private bool CanDeleteMallCommandExecute(object parameters) => IsSelectedMall;
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

        #region Просмотр ТЦ
        public ICommand ViewMallCommand { get; }
        private bool CanViewMallCommandExecute(object parameters) => IsSelectedMall;
        private void OnViewMallCommandExecuted(object parameters)
        {
            UpdatePavilions();
            Manager.Instance.MainFrameNavigate(new ViewingPavilionPage());
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
        private ActionEntities _currentMallActionEntities = ActionEntities.None;
        /// <summary>
        /// ActionEntities
        /// </summary>
        public ActionEntities CurrentMallActionEntities
        {
            get => _currentMallActionEntities;
            set
            {
                Set(ref _currentMallActionEntities, value);
                UpdateMallStatuses();
                if (CurrentMallActionEntities == ActionEntities.Add)
                {
                    MallButtonName = "Добавить";
                    LoadedImage = null;
                    CurrentMall = new Mall();
                }
                else if (CurrentMallActionEntities == ActionEntities.Change)
                {
                    MallButtonName = "Изменить";
                    CurrentMall = (
                        from m in Manager.Instance.Context.Mall
                        where m.mall_id == SelectedMall.mall_id
                        select m
                    ).FirstOrDefault();
                    SelectedMallStatus = SelectedMall.status_name;
                    if (CurrentMall.photo != null)
                    {
                        LoadedImage = Tools.BytesToImage(CurrentMall.photo);
                    }
                }
            }
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

        #region MallButtonName
        private string _mallButtonName;
        /// <summary>
        /// MallButtonName
        /// </summary>
        public string MallButtonName
        {
            get => _mallButtonName;
            set => Set(ref _mallButtonName, value);
        }
        #endregion

        #region Команда
        public ICommand MallExecuteCommand { get; }
        private bool CanMallExecuteCommandExecute(object parameters) => true;
        private void OnMallExecuteCommandExecuted(object parameters)
        {
            if (CurrentMall.cost < 0 ||
                CurrentMall.number_of_pavilion < 0 ||
                CurrentMall.value_added_factor < 0.1 ||
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
                    switch (CurrentMallActionEntities)
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
            SelectedCity = AllNameSorting;
            UpdateMalls();
            UpdateCities();
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
        private ObservableCollection<string> _mallStatusesSorting;
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
                Set(ref _selectedMallStatusSorting, value);
                UpdateMalls();
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
                Set(ref _selectedCity, value);
                UpdateMalls();
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

        #region GetMalls
        private ObservableCollection<MallItem> GetMalls()
        {
            ObservableCollection<MallItem> data = new ObservableCollection<MallItem>();
            if (SelectedMallStatusSorting == AllNameSorting)
            {
                if (SelectedCity == AllNameSorting)
                {
                    data = new ObservableCollection<MallItem>(
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
                else
                {
                    data = new ObservableCollection<MallItem>(
                    from mall in Manager.Instance.Context.Mall
                    join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                    orderby mall.city, ms.status_name
                    where ms.status_name != DeleteNameSorting &&
                          mall.city == SelectedCity
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
            else
            {
                // SelectedMallStatusSorting != AllNameSorting
                if (SelectedCity == AllNameSorting)
                {
                    data = new ObservableCollection<MallItem>(
                        from mall in Manager.Instance.Context.Mall
                        join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                        orderby mall.city, ms.status_name
                        where ms.status_name == SelectedMallStatusSorting
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
                        }
                    );
                }
                else
                {
                    data = new ObservableCollection<MallItem>(
                        from mall in Manager.Instance.Context.Mall
                        join ms in Manager.Instance.Context.Mall_statuses on mall.status_id equals ms.status_id
                        orderby mall.city, ms.status_name
                        where ms.status_name == SelectedMallStatusSorting &&
                              mall.city == SelectedCity
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
                        }
                    );
                }
            }
            return data;
        }
        #endregion

        #region UpdateMalls
        private void UpdateMalls()
        {
            Malls = GetMalls();
        }
        #endregion

        #region GetMallStatuses
        private ObservableCollection<string> GetMallStatuses()
        {
            return new ObservableCollection<string>(
                from ms in Manager.Instance.Context.Mall_statuses
                orderby ms.status_name
                select ms.status_name
                );
        }
        #endregion

        #region UpdateMallStatuses
        private void UpdateMallStatuses()
        {
            MallStatuses = GetMallStatuses();
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
            return new ObservableCollection<string>(
                (
                    from m in Manager.Instance.Context.Mall
                    orderby m.city
                    where m.status_id != (from ms in Manager.Instance.Context.Mall_statuses
                                          where ms.status_name == DeleteNameSorting
                                          select ms.status_id).FirstOrDefault()
                    select m.city
                ).Distinct().ToList()
            )
            {
                AllNameSorting
            };
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


        #region ViewingPavilionPage

        #region Павильоны название
        private string _pavilionTitleName = "Павильоны";
        /// <summary>
        /// Павильоны название
        /// </summary>
        public string PavilionTitleName
        {
            get => _pavilionTitleName;
            set => Set(ref _pavilionTitleName, value);
        }
        #endregion

        #region Павильоны
        private ObservableCollection<PavilionItem> _pavilions;
        /// <summary>
        /// Павильоны
        /// </summary>
        public ObservableCollection<PavilionItem> Pavilions
        {
            get => _pavilions;
            set => Set(ref _pavilions, value);
        }
        #endregion

        #region Выбранный Павильон
        private PavilionItem _selectedPavilion;
        /// <summary>
        /// Выбранный Павильон
        /// </summary>
        public PavilionItem SelectedPavilion
        {
            get => _selectedPavilion;
            set
            {
                IsSelectedPavilion = value != null;
                Set(ref _selectedPavilion, value);
            }
        }
        #endregion

        #region Павильон выбран
        private bool _isSelectedPavilion = false;
        /// <summary>
        /// Павильон выбран
        /// </summary>
        public bool IsSelectedPavilion
        {
            get => _isSelectedPavilion;
            set => Set(ref _isSelectedPavilion, value);
        }
        #endregion

        #region UpdatePavilions
        private void UpdatePavilions()
        {
            Pavilions = GetPavilions();
        }
        #endregion

        #region GetPavilions
        private ObservableCollection<PavilionItem> GetPavilions()
        {
            ObservableCollection<PavilionItem> data = new ObservableCollection<PavilionItem>();
            if (SelectedMall == null)
            {
                return null;
            }
            else if (SelectedPavilionStatusSorting == AllNameSorting)
            {
                data = new ObservableCollection<PavilionItem>(
                from p in Manager.Instance.Context.Pavilion
                join ps in Manager.Instance.Context.Pavilion_statuses on p.status_id equals ps.status_id
                join m in Manager.Instance.Context.Mall on p.mall_id equals m.mall_id
                join ms in Manager.Instance.Context.Mall_statuses on m.status_id equals ms.status_id
                where ps.status_name != DeleteNameSorting &&
                      p.floor == Floor &&
                      p.square >= MinSquare &&
                      p.square <= MaxSquare &&
                      m.mall_name == SelectedMall.mall_name
                select new PavilionItem
                {
                    mall_id = m.mall_id,
                    mall_status_id = m.status_id,
                    mall_status_name = ms.status_name,
                    mall_name = m.mall_name,
                    floor = p.floor,
                    pavilion_number = p.pavilion_number,
                    square = p.square,
                    pavilion_status_id = p.status_id,
                    pavilion_status_name = ps.status_name,
                    cost_per_square_meter = p.cost_per_square_meter,
                    value_added_factor = p.value_added_factor
                });
            }
            else
            {
                data = new ObservableCollection<PavilionItem>(
                    from p in Manager.Instance.Context.Pavilion
                    join ps in Manager.Instance.Context.Pavilion_statuses on p.status_id equals ps.status_id
                    join m in Manager.Instance.Context.Mall on p.mall_id equals m.mall_id
                    join ms in Manager.Instance.Context.Mall_statuses on m.status_id equals ms.status_id
                    where ps.status_name != DeleteNameSorting &&
                          ps.status_name == SelectedPavilionStatusSorting &&
                          p.floor == Floor &&
                          p.square >= MinSquare &&
                          p.square <= MaxSquare &&
                          m.mall_name == SelectedMall.mall_name
                    select new PavilionItem
                    {
                        mall_id = m.mall_id,
                        mall_status_id = m.status_id,
                        mall_status_name = ms.status_name,
                        mall_name = m.mall_name,
                        floor = p.floor,
                        pavilion_number = p.pavilion_number,
                        square = p.square,
                        pavilion_status_id = p.status_id,
                        pavilion_status_name = ps.status_name,
                        cost_per_square_meter = p.cost_per_square_meter,
                        value_added_factor = p.value_added_factor
                    });
            }
            return data;
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

        #region GetNewPavilion
        private Pavilion GetNewPavilion()
        {
            return new Pavilion()
            {
                pavilion_number = ""
            };
        }
        #endregion

        #region Добавить павильон
        public ICommand AddPavilionCommand { get; }
        private bool CanAddPavilionCommandExecute(object parameters) => true;
        private void OnAddPavilionCommandExecuted(object parameters)
        {
            CurrentActionEntities = ActionEntities.Add;
            Manager.Instance.MainFrameNavigate(new PavilionPage());
        }
        #endregion

        #region Изменить павильон
        public ICommand ChangePavilionCommand { get; }
        private bool CanChangePavilionCommandExecute(object parameters) => IsSelectedPavilion;
        private void OnChangePavilionCommandExecuted(object parameters)
        {
            CurrentActionEntities = ActionEntities.Change;
            Manager.Instance.MainFrameNavigate(new PavilionPage());
        }
        #endregion

        #region Обновить павильоны
        public ICommand UpdatePavilionsCommand { get; }
        private bool CanUpdatePavilionsCommandExecute(object parameters) => true;
        private void OnUpdatePavilionsCommandExecuted(object parameters)
        {
            UpdatePavilions();
        }
        #endregion

        #region Удалить павильон
        public ICommand DeletePavilionCommand { get; }
        private bool CanDeletePavilionCommandExecute(object parameters) => IsSelectedPavilion;
        private void OnDeletePavilionCommandExecuted(object parameters)
        {
            try
            {
                Pavilion pavilion = (
                    from p in Manager.Instance.Context.Pavilion
                    where p.mall_id == SelectedPavilion.mall_id &&
                          p.pavilion_number == SelectedPavilion.pavilion_number
                    select p
                ).FirstOrDefault();
                pavilion.status_id = (
                    from m in Manager.Instance.Context.Pavilion_statuses
                    where m.status_name == DeleteNameSorting
                    select m.status_id
                ).FirstOrDefault();
                Manager.Instance.Context.SaveChanges();
                Pavilions.Remove(SelectedPavilion); // или UpdatePavilions();
                MessageBox.Show($"Павильон удалён.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка :\n{e}");
            }
        }
        #endregion

        #region Текущий павильон
        private Pavilion _currentPavilion;
        /// <summary>
        /// Текущий павильон
        /// </summary>
        public Pavilion CurrentPavilion
        {
            get => _currentPavilion;
            set => Set(ref _currentPavilion, value);
        }
        #endregion

        #region Статусы павильонов
        private ObservableCollection<string> _PavilionStatuses;
        /// <summary>
        /// Статусы павильонов
        /// </summary>
        public ObservableCollection<string> PavilionStatuses
        {
            get => _PavilionStatuses;
            set => Set(ref _PavilionStatuses, value);
        }
        #endregion

        #region Выбранный статус павильона
        private string _selectedPavilionStatus;
        /// <summary>
        /// Выбранный статус павильона
        /// </summary>
        public string SelectedPavilionStatus
        {
            get => _selectedPavilionStatus;
            set => Set(ref _selectedPavilionStatus, value);
        }
        #endregion

        #region UpdatePavilionStatuses
        private void UpdatePavilionStatuses()
        {
            PavilionStatuses = GetPavilionStatuses();
        }
        #endregion

        #region GetPavilionStatuses
        private ObservableCollection<string> GetPavilionStatuses()
        {
            return new ObservableCollection<string>(
                from p in Manager.Instance.Context.Pavilion_statuses
                select p.status_name
            );
        }
        #endregion

        #region Названия павильонов
        private ObservableCollection<string> _pavilionNames;
        /// <summary>
        /// Названия павильонов
        /// </summary>
        public ObservableCollection<string> PavilionNames
        {
            get => _pavilionNames;
            set => Set(ref _pavilionNames, value);
        }
        #endregion

        #region Выбранное название павильона
        private string _selectedMallPavilionName;
        /// <summary>
        /// Выбранное название павильона
        /// </summary>
        public string SelectedMallPavilionName
        {
            get => _selectedMallPavilionName;
            set => Set(ref _selectedMallPavilionName, value);
        }
        #endregion

        #region UpdatePavilionNames
        private void UpdatePavilionNames()
        {
            PavilionNames = GetPavilionNames();
        }
        #endregion

        #region GetPavilionNames
        private ObservableCollection<string> GetPavilionNames()
        {
            return new ObservableCollection<string>(
                from m in Manager.Instance.Context.Mall
                select m.mall_name
            );
        }
        #endregion


        #region Сортировка статусов павильонов
        private ObservableCollection<string> _pavilionStatusesSorting;
        /// <summary>
        /// Сортировка статусов павильонов
        /// </summary>
        public ObservableCollection<string> PavilionStatusesSorting
        {
            get => _pavilionStatusesSorting;
            set => Set(ref _pavilionStatusesSorting, value);
        }
        #endregion

        #region Выбранная сортировка статуса
        private string _selectedPavilionStatusSorting;
        /// <summary>
        /// Выбранная сортировка статуса
        /// </summary>
        public string SelectedPavilionStatusSorting
        {
            get => _selectedPavilionStatusSorting;
            set
            {
                Set(ref _selectedPavilionStatusSorting, value);
                UpdatePavilions();
            }
        }
        #endregion


        #region GetPavilionStatusSorting
        private ObservableCollection<string> GetPavilionStatusSorting()
        {
            return new ObservableCollection<string>(
                from p in Manager.Instance.Context.Pavilion_statuses
                orderby p.status_name
                select p.status_name
            );
        }
        #endregion

        #region UpdatePavilionStatusSorting
        private void UpdatePavilionStatusSorting()
        {
            PavilionStatusesSorting = GetPavilionStatusSorting();
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
            set
            {
                Set(ref _currentActionEntities, value);

                if (_currentActionEntities == ActionEntities.Add)
                {
                    CurrentPavilion = GetNewPavilion();
                    ButtonName = "Добавить";
                }
                else if (_currentActionEntities == ActionEntities.Change)
                {
                    ButtonName = "Изменить";
                    CurrentPavilion = (
                        from p in Manager.Instance.Context.Pavilion
                        where p.mall_id == SelectedPavilion.mall_id &&
                                p.pavilion_number == SelectedPavilion.pavilion_number
                        select p
                    ).FirstOrDefault();
                    SelectedPavilionStatus = SelectedPavilion.pavilion_status_name;
                    SelectedMallPavilionName = SelectedPavilion.mall_name;
                }
            }
        }
        #endregion

        #region Команда
        public ICommand ExecuteCommand { get; }
        private bool CanExecuteCommandExecute(object parameters) => true;
        private void OnExecuteCommandExecuted(object parameters)
        {
            if (CurrentPavilion.floor < 0 ||
                CurrentPavilion.square < 0 ||
                CurrentPavilion.value_added_factor < 0 ||
                CurrentPavilion.cost_per_square_meter < (decimal)0.1)
            {
                MessageBox.Show($"Числовые поля должны быть положительными.");
            }
            else if (CurrentPavilion.pavilion_number != null &&
                     string.IsNullOrEmpty(CurrentPavilion.pavilion_number.Trim()) ||
                     SelectedPavilionStatus == null)
            {
                MessageBox.Show($"Заполните все поля.");
            }
            else
            {
                try
                {
                    CurrentPavilion.status_id = (
                        from ps in Manager.Instance.Context.Pavilion_statuses
                        where ps.status_name == SelectedPavilionStatus
                        select ps.status_id
                    ).FirstOrDefault();
                    CurrentPavilion.mall_id = (
                        from m in Manager.Instance.Context.Mall
                        where m.mall_id == SelectedMall.mall_id
                        select m.mall_id
                    ).FirstOrDefault();
                    CurrentPavilion.pavilion_number = CurrentPavilion.pavilion_number.Trim();
                    switch (CurrentActionEntities)
                    {
                        case ActionEntities.Add:
                            Manager.Instance.Context.Pavilion.Add(CurrentPavilion);
                            break;
                        case ActionEntities.Change:
                            break;
                    }
                    Manager.Instance.Context.SaveChanges();
                    switch (CurrentActionEntities)
                    {
                        case ActionEntities.Add:
                            MessageBox.Show($"Павильон добавлен.");
                            break;
                        case ActionEntities.Change:
                            MessageBox.Show($"Павильон изменён.");
                            break;
                    }
                    Manager.Instance.MainFrameNavigate(new ViewingPavilionPage());
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Ошибка :\n{e}");
                }
            }
            SelectedPavilionStatusSorting = AllNameSorting;
            UpdatePavilions();
        }
        #endregion

        #region Этаж
        private int _floor = 2;
        /// <summary>
        /// Этаж
        /// </summary>
        public int Floor
        {
            get => _floor;
            set
            {
                if (value >= 0)
                {
                    Set(ref _floor, value);
                    UpdatePavilions();
                }
            }
        }
        #endregion

        #region Мин площадь
        private decimal _minSquare = 0;
        /// <summary>
        /// Min площадь
        /// </summary>
        public decimal MinSquare
        {
            get => _minSquare;
            set
            {
                if (value >= 0)
                {
                    if (value <= MaxSquare)
                    {
                        Set(ref _minSquare, value);
                    }
                    else
                    {
                        Set(ref _minSquare, MaxSquare);
                    }
                    UpdatePavilions();
                }
            }
        }
        #endregion

        #region Мин площадь
        private decimal _maxSquare = 1000;
        /// <summary>
        /// Max площадь
        /// </summary>
        public decimal MaxSquare
        {
            get => _maxSquare;
            set
            {
                if (value >= 0)
                {
                    if (value >= MinSquare)
                    {
                        Set(ref _maxSquare, value);
                    }
                    else
                    {
                        Set(ref _maxSquare, MinSquare);
                    }
                    UpdatePavilions();
                }
            }
        }
        #endregion


        #endregion


        #region PavilionRentalPage


        #region Просмотр ТЦ
        public ICommand RentPavilionCommand { get; }
        private bool CanRentPavilionCommandExecute(object parameters) => IsSelectedPavilion;
        private void OnRentPavilionCommandExecuted(object parameters)
        {
            Manager.Instance.MainFrameNavigate(new PavilionRentalPage());
        }
        #endregion

        #region Действие аренда
        private string _rentActionName = "Аренда";
        /// <summary>
        /// Действие аренда
        /// </summary>
        public string RentActionName
        {
            get => _rentActionName;
            set => Set(ref _rentActionName, value);
        }
        #endregion

        #region Действие забронировать
        private string _bookActionName = "Бронь";
        /// <summary>
        /// Действие забронировать
        /// </summary>
        public string BookActionName
        {
            get => _bookActionName;
            set => Set(ref _bookActionName, value);
        }
        #endregion

        #region Действия аренда/бронь
        private ObservableCollection<string> _rentActions;
        /// <summary>
        /// Действия аренда/бронь
        /// </summary>
        public ObservableCollection<string> RentActions
        {
            get => _rentActions;
            set => Set(ref _rentActions, value);
        }
        #endregion


        #region Выбранное действие аренда/бронь
        private string _selectedRentAction;
        /// <summary>
        /// Выбранное действие аренда/бронь
        /// </summary>
        public string SelectedRentAction
        {
            get => _selectedRentAction;
            set
            {
                Set(ref _selectedRentAction, value);
                if (_selectedRentAction == RentActionName)
                {
                    PavilionRentalButtonName = "Арендовать";
                }
                else if (_selectedRentAction == BookActionName)
                {
                    PavilionRentalButtonName = "Забронировать";
                }
            }
        }
        #endregion

        #region GetRentActions
        private ObservableCollection<string> GetRentActions()
        {
            return new ObservableCollection<string>()
            {
                RentActionName,
                BookActionName
            };
        }
        #endregion

        #region UpdateRentActions
        private void UpdateRentActions()
        {
            RentActions = GetRentActions();
        }
        #endregion

        #region Начало аренды
        private DateTime? _rentalStartDate = DateTime.Today;
        /// <summary>
        /// Начало аренды
        /// </summary>
        public DateTime? RentalStartDate
        {
            get => _rentalStartDate;
            set
            {
                Set(ref _rentalStartDate, value);
                MinimumRentalEndDate = _rentalStartDate;
            }
        }
        #endregion

        #region Минимальная выбираемая дата для начала аренды
        private DateTime? _minimumRentalStartDate = DateTime.Today;
        /// <summary>
        /// Минимальная выбираемая дата для начала аренды
        /// </summary>
        public DateTime? MinimumRentalStartDate
        {
            get => _minimumRentalStartDate;
            set => Set(ref _minimumRentalStartDate, value);
        }
        #endregion

        #region Максимальная выбираемая дата для начала аренды
        private DateTime? _maximumRentalStartDate;
        /// <summary>
        /// Максимальная выбираемая дата для начала аренды
        /// </summary>
        public DateTime? MaximumRentalStartDate
        {
            get => _maximumRentalStartDate;
            set => Set(ref _maximumRentalStartDate, value);
        }
        #endregion

        #region Конец аренды
        private DateTime? _rentalEndDate;
        /// <summary>
        /// Конец аренды
        /// </summary>
        public DateTime? RentalEndDate
        {
            get => _rentalEndDate;
            set
            {
                Set(ref _rentalEndDate, value);
                MaximumRentalStartDate = _rentalEndDate;
            }
        }
        #endregion

        #region Минимальная выбираемая дата для конца аренды
        private DateTime? _minimumRentalEndDate = DateTime.Today;
        /// <summary>
        /// Минимальная выбираемая дата для конца аренды
        /// </summary>
        public DateTime? MinimumRentalEndDate
        {
            get => _minimumRentalEndDate;
            set => Set(ref _minimumRentalEndDate, value);
        }
        #endregion

        #region Максимальная выбираемая дата для начала аренды
        private DateTime? _maximumRentalEndDate;
        /// <summary>
        /// Максимальная выбираемая дата для конца аренды
        /// </summary>
        public DateTime? MaximumRentalEndDate
        {
            get => _maximumRentalEndDate;
            set => Set(ref _maximumRentalEndDate, value);
        }
        #endregion

        #region Арендаторы
        private ObservableCollection<string> _tenants;
        /// <summary>
        /// Арендаторы
        /// </summary>
        public ObservableCollection<string> Tenants
        {
            get => _tenants;
            set => Set(ref _tenants, value);
        }
        #endregion

        #region Выбранный арендатор
        private string _selectedTenant;
        /// <summary>
        /// Выбранный арендатор
        /// </summary>
        public string SelectedTenant
        {
            get => _selectedTenant;
            set => Set(ref _selectedTenant, value);
        }
        #endregion

        #region GetTenants
        private ObservableCollection<string> GetTenants()
        {
            return new ObservableCollection<string>(
                from t in Manager.Instance.Context.Tenants
                select t.company_name
            );
        }
        #endregion

        #region UpdateTenants
        private void UpdateTenants()
        {
            Tenants = GetTenants();
        }
        #endregion



        #region Сотрудники
        private ObservableCollection<string> _employees;
        /// <summary>
        /// Сотрудники
        /// </summary>
        public ObservableCollection<string> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        #endregion

        #region Выбранный сотрудник
        private string _selectedEmployee;
        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        public string SelectedEmployee
        {
            get => _selectedEmployee;
            set => Set(ref _selectedEmployee, value);
        }
        #endregion

        #region GetEmployees
        private ObservableCollection<string> GetEmployees()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            foreach (Employees em in from t in Manager.Instance.Context.Employees
                                     select t)
            {
                result.Add($"{em.employe_id} {em.surname} {em.name} {em.patronymic}");
            }
            return result;
        }
        #endregion

        #region UpdateEmployees
        private void UpdateEmployees()
        {
            Employees = GetEmployees();
        }
        #endregion


        #region PavilionRentalButtonName
        private string _pavilionRentalButtonName;
        /// <summary>
        /// PavilionRentalButtonName
        /// </summary>
        public string PavilionRentalButtonName
        {
            get => _pavilionRentalButtonName;
            set => Set(ref _pavilionRentalButtonName, value);
        }
        #endregion

        #region Удалить павильон
        public ICommand PavilionRentalCommand { get; }
        private bool CanPavilionRentalCommandExecute(object parameters) => true;
        private void OnPavilionRentalCommandExecuted(object parameters)
        {
            if (SelectedRentAction != null &&
                RentalStartDate != null &&
                SelectedPavilion != null &&
                RentalEndDate != null &&
                SelectedTenant != null &&
                SelectedEmployee != null)
            {

                try
                {
                    int status_action = (SelectedRentAction == RentActionName) ? 0 : 1;
                    string pavilion_number = SelectedPavilion.pavilion_number;
                    long mall_id = SelectedPavilion.mall_id;
                    long tenant_id = (from t in Manager.Instance.Context.Tenants
                                      where t.company_name == SelectedTenant
                                      select t.tenant_id).FirstOrDefault();
                    long employee_id = long.Parse(SelectedEmployee.Split(' ')[0]);
                    Manager.Instance.Context.Database.ExecuteSqlCommand
                        ($@"exec RentOrBookPavilionInMall @status_action, 
                                                      @pavilion_number, 
                                                      @mall_id, 
                                                      @start_date, 
                                                      @end_date, 
                                                      @tenant_id, 
                                                      @employee_id",
                        new SqlParameter("@status_action", status_action),
                        new SqlParameter("@pavilion_number", pavilion_number),
                        new SqlParameter("@mall_id", mall_id),
                        new SqlParameter("@start_date", RentalStartDate),
                        new SqlParameter("@end_date", RentalEndDate),
                        new SqlParameter("@tenant_id", tenant_id),
                        new SqlParameter("@employee_id", employee_id));
                    MessageBox.Show($"Павильон {((SelectedRentAction == RentActionName) ? "арендован" : "забронирован")}.");
                    Manager.Instance.MainFrameNavigate(new ViewingPavilionPage());
                    UpdatePavilions();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Ошибка :\n{e}");
                }
            }
            else
            {
                MessageBox.Show($"Заполните все поля.");
            }
        }
        #endregion


        #endregion


        #region Конструктор
        public ViewingMallPageViewModel()
        {
            DeleteMallCommand = new LambdaCommand(OnDeleteMallCommandExecuted, CanDeleteMallCommandExecute);
            AddMallCommand = new LambdaCommand(OnAddMallCommandExecuted, CanAddMallCommandExecute);
            ChangeMallCommand = new LambdaCommand(OnChangeMallCommandExecuted, CanChangeMallCommandExecute);
            ViewMallCommand = new LambdaCommand(OnViewMallCommandExecuted, CanViewMallCommandExecute);
            UpdateMalls();

            MallExecuteCommand = new LambdaCommand(OnMallExecuteCommandExecuted, CanMallExecuteCommandExecute);
            LoadPhotoCommand = new LambdaCommand(OnLoadPhotoCommandExecuted, CanLoadPhotoCommandExecute);
            UpdateMallStatuses();

            MallStatusesSorting = GetMallStatuses();

            // убираю сортировку по удаленным ТЦ
            MallStatusesSorting.Remove(DeleteNameSorting);
            // добавляю общую сортировку
            MallStatusesSorting.Add(AllNameSorting);
            // текущим выбранным ставим все
            SelectedMallStatusSorting = AllNameSorting;

            UpdateCities();
            //// текущим выбранным ставим все
            SelectedCity = AllNameSorting;

            #region ViewingPavilionPageViewModel

            AddPavilionCommand = new LambdaCommand(OnAddPavilionCommandExecuted, CanAddPavilionCommandExecute);
            ChangePavilionCommand = new LambdaCommand(OnChangePavilionCommandExecuted, CanChangePavilionCommandExecute);
            DeletePavilionCommand = new LambdaCommand(OnDeletePavilionCommandExecuted, CanDeletePavilionCommandExecute);
            ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecute);
            UpdatePavilionsCommand = new LambdaCommand(OnUpdatePavilionsCommandExecuted, CanUpdatePavilionsCommandExecute);

            UpdatePavilionStatuses();
            UpdatePavilionStatusSorting();
            // убираю сортировку по удаленным ТЦ
            PavilionStatusesSorting.Remove(DeleteNameSorting);
            // добавляю общую сортировку
            PavilionStatusesSorting.Add(AllNameSorting);
            // текущим выбранным ставим все
            SelectedPavilionStatusSorting = AllNameSorting;

            UpdatePavilions();
            UpdatePavilionNames();
            CurrentPavilion = GetNewPavilion();
            #endregion

            #region PavilionRentalPage

            RentPavilionCommand = new LambdaCommand(OnRentPavilionCommandExecuted, CanRentPavilionCommandExecute);
            PavilionRentalCommand = new LambdaCommand(OnPavilionRentalCommandExecuted, CanPavilionRentalCommandExecute);
            UpdateRentActions();
            UpdateTenants();
            UpdateEmployees();
            SelectedRentAction = RentActionName;

            RentalEndDate = DateTime.Today.AddDays(1);

            #endregion
        }
        #endregion
    }
}