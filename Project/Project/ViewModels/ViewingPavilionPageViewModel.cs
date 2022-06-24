using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class ViewingPavilionPageViewModel : ViewModel
    {
        //#region Павильоны название
        //private string _pavilionTitleName = "Павильоны";
        ///// <summary>
        ///// Павильоны название
        ///// </summary>
        //public string PavilionTitleName
        //{
        //    get => _pavilionTitleName;
        //    set => Set(ref _pavilionTitleName, value);
        //}
        //#endregion

        //#region Павильоны
        //private ObservableCollection<PavilionItem> _pavilions;
        ///// <summary>
        ///// Павильоны
        ///// </summary>
        //public ObservableCollection<PavilionItem> Pavilions
        //{
        //    get => _pavilions;
        //    set => Set(ref _pavilions, value);
        //}
        //#endregion

        //#region Выбранный Павильон
        //private PavilionItem _selectedPavilion;
        ///// <summary>
        ///// Выбранный Павильон
        ///// </summary>
        //public PavilionItem SelectedPavilion
        //{
        //    get => _selectedPavilion;
        //    set
        //    {
        //        IsSelected = value != null;
        //        Set(ref _selectedPavilion, value);
        //    }
        //}
        //#endregion

        //#region Павильон выбран
        //private bool _isSelected = false;
        ///// <summary>
        ///// Павильон выбран
        ///// </summary>
        //public bool IsSelected
        //{
        //    get => _isSelected;
        //    set => Set(ref _isSelected, value);
        //}
        //#endregion

        //#region UpdatePavilions
        //private void UpdatePavilions()
        //{
        //    Pavilions = GetPavilions();
        //}
        //#endregion

        //#region GetPavilions
        //private ObservableCollection<PavilionItem> GetPavilions()
        //{
        //    ObservableCollection<PavilionItem> data = new ObservableCollection<PavilionItem>();
        //    if (SelectedPavilionStatusSorting == AllNameSorting)
        //    {
        //        data = new ObservableCollection<PavilionItem>(
        //        from p in Manager.Instance.Context.Pavilion
        //        join ps in Manager.Instance.Context.Pavilion_statuses on p.status_id equals ps.status_id
        //        join m in Manager.Instance.Context.Mall on p.mall_id equals m.mall_id
        //        join ms in Manager.Instance.Context.Mall_statuses on m.status_id equals ms.status_id
        //        where ps.status_name != DeleteNameSorting &&
        //              p.floor == Floor &&
        //              p.square >= MinSquare &&
        //              p.square <= MaxSquare
        //        select new PavilionItem
        //        {
        //            mall_id = m.mall_id,
        //            mall_status_id = m.status_id,
        //            mall_status_name = ms.status_name,
        //            mall_name = m.mall_name,
        //            floor = p.floor,
        //            pavilion_number = p.pavilion_number,
        //            square = p.square,
        //            pavilion_status_id = p.status_id,
        //            pavilion_status_name = ps.status_name,
        //            cost_per_square_meter = p.cost_per_square_meter,
        //            value_added_factor = p.value_added_factor
        //        });
        //    }
        //    else
        //    {
        //        data = new ObservableCollection<PavilionItem>(
        //            from p in Manager.Instance.Context.Pavilion
        //            join ps in Manager.Instance.Context.Pavilion_statuses on p.status_id equals ps.status_id
        //            join m in Manager.Instance.Context.Mall on p.mall_id equals m.mall_id
        //            join ms in Manager.Instance.Context.Mall_statuses on m.status_id equals ms.status_id
        //            where ps.status_name != DeleteNameSorting &&
        //                  ps.status_name == SelectedPavilionStatusSorting &&
        //                  p.floor == Floor &&
        //                  p.square >= MinSquare &&
        //                  p.square <= MaxSquare
        //            select new PavilionItem
        //            {
        //                mall_id = m.mall_id,
        //                mall_status_id = m.status_id,
        //                mall_status_name = ms.status_name,
        //                mall_name = m.mall_name,
        //                floor = p.floor,
        //                pavilion_number = p.pavilion_number,
        //                square = p.square,
        //                pavilion_status_id = p.status_id,
        //                pavilion_status_name = ps.status_name,
        //                cost_per_square_meter = p.cost_per_square_meter,
        //                value_added_factor = p.value_added_factor
        //            });
        //    }
        //    return data;
        //}
        //#endregion

        //#region ButtonName
        //private string _ButtonName;
        ///// <summary>
        ///// ButtonName
        ///// </summary>
        //public string ButtonName
        //{
        //    get => _ButtonName;
        //    set => Set(ref _ButtonName, value);
        //}
        //#endregion

        //#region GetNewPavilion
        //private Pavilion GetNewPavilion()
        //{
        //    return new Pavilion()
        //    {
        //        pavilion_number = ""
        //    };
        //}
        //#endregion

        //#region Добавить павильон
        //public ICommand AddPavilionCommand { get; }
        //private bool CanAddPavilionCommandExecute(object parameters) => true;
        //private void OnAddPavilionCommandExecuted(object parameters)
        //{
        //    CurrentActionEntities = ActionEntities.Add;
        //    Manager.Instance.MainFrameNavigate(new PavilionPage());
        //}
        //#endregion

        //#region Изменить павильон
        //public ICommand ChangePavilionCommand { get; }
        //private bool CanChangePavilionCommandExecute(object parameters) => IsSelected;
        //private void OnChangePavilionCommandExecuted(object parameters)
        //{
        //    CurrentActionEntities = ActionEntities.Change;
        //    Manager.Instance.MainFrameNavigate(new PavilionPage());
        //}
        //#endregion

        //#region Обновить павильоны
        //public ICommand UpdatePavilionsCommand { get; }
        //private bool CanUpdatePavilionsCommandExecute(object parameters) => true;
        //private void OnUpdatePavilionsCommandExecuted(object parameters)
        //{
        //    UpdatePavilions();
        //}
        //#endregion

        //#region Удалить павильон
        //public ICommand DeletePavilionCommand { get; }
        //private bool CanDeletePavilionCommandExecute(object parameters) => IsSelected;
        //private void OnDeletePavilionCommandExecuted(object parameters)
        //{
        //    try
        //    {
        //        Pavilion pavilion = (
        //            from p in Manager.Instance.Context.Pavilion
        //            where p.mall_id == SelectedPavilion.mall_id
        //            select p
        //        ).FirstOrDefault();
        //        pavilion.status_id = (
        //            from m in Manager.Instance.Context.Pavilion_statuses
        //            where m.status_name == DeleteNameSorting
        //            select m.status_id
        //        ).FirstOrDefault();
        //        Manager.Instance.Context.SaveChanges();
        //        Pavilions.Remove(SelectedPavilion); // или UpdatePavilions();
        //        MessageBox.Show($"Павильон удалён.");
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show($"Ошибка :\n{e}");
        //    }
        //}
        //#endregion

        //#region Текущий павильон
        //private Pavilion _currentPavilion;
        ///// <summary>
        ///// Текущий павильон
        ///// </summary>
        //public Pavilion CurrentPavilion
        //{
        //    get => _currentPavilion;
        //    set => Set(ref _currentPavilion, value);
        //}
        //#endregion

        //#region Статусы павильонов
        //private ObservableCollection<string> _PavilionStatuses;
        ///// <summary>
        ///// Статусы павильонов
        ///// </summary>
        //public ObservableCollection<string> PavilionStatuses
        //{
        //    get => _PavilionStatuses;
        //    set => Set(ref _PavilionStatuses, value);
        //}
        //#endregion

        //#region Выбранный статус павильона
        //private string _selectedPavilionStatus;
        ///// <summary>
        ///// Выбранный статус павильона
        ///// </summary>
        //public string SelectedPavilionStatus
        //{
        //    get => _selectedPavilionStatus;
        //    set => Set(ref _selectedPavilionStatus, value);
        //}
        //#endregion

        //#region UpdatePavilionStatuses
        //private void UpdatePavilionStatuses()
        //{
        //    PavilionStatuses = GetPavilionStatuses();
        //}
        //#endregion

        //#region GetPavilionStatuses
        //private static ObservableCollection<string> GetPavilionStatuses()
        //{
        //    return new ObservableCollection<string>(
        //        from p in Manager.Instance.Context.Pavilion_statuses
        //        select p.status_name
        //    );
        //}
        //#endregion

        //#region Названия павильонов
        //private ObservableCollection<string> _pavilionNames;
        ///// <summary>
        ///// Названия павильонов
        ///// </summary>
        //public ObservableCollection<string> PavilionNames
        //{
        //    get => _pavilionNames;
        //    set => Set(ref _pavilionNames, value);
        //}
        //#endregion

        //#region Выбранное название павильона
        //private string _selectedMallPavilionName;
        ///// <summary>
        ///// Выбранное название павильона
        ///// </summary>
        //public string SelectedMallPavilionName
        //{
        //    get => _selectedMallPavilionName;
        //    set => Set(ref _selectedMallPavilionName, value);
        //}
        //#endregion

        //#region UpdatePavilionNames
        //private void UpdatePavilionNames()
        //{
        //    PavilionNames = GetPavilionNames();
        //}
        //#endregion

        //#region GetPavilionNames
        //private static ObservableCollection<string> GetPavilionNames()
        //{
        //    return new ObservableCollection<string>(
        //        from m in Manager.Instance.Context.Mall
        //        select m.mall_name
        //    );
        //}
        //#endregion


        //#region Сортировка статусов павильонов
        //private ObservableCollection<string> _pavilionStatusesSorting;
        ///// <summary>
        ///// Сортировка статусов павильонов
        ///// </summary>
        //public ObservableCollection<string> PavilionStatusesSorting
        //{
        //    get => _pavilionStatusesSorting;
        //    set => Set(ref _pavilionStatusesSorting, value);
        //}
        //#endregion

        //#region Выбранная сортировка статуса
        //private string _selectedPavilionStatusSorting;
        ///// <summary>
        ///// Выбранная сортировка статуса
        ///// </summary>
        //public string SelectedPavilionStatusSorting
        //{
        //    get => _selectedPavilionStatusSorting;
        //    set
        //    {
        //        Set(ref _selectedPavilionStatusSorting, value);
        //        UpdatePavilions();
        //    }
        //}
        //#endregion


        //#region GetPavilionStatusSorting
        //private static ObservableCollection<string> GetPavilionStatusSorting()
        //{
        //    return new ObservableCollection<string>(
        //        from p in Manager.Instance.Context.Pavilion_statuses
        //        orderby p.status_name
        //        select p.status_name
        //    );
        //}
        //#endregion

        //#region UpdatePavilionStatusSorting
        //private void UpdatePavilionStatusSorting()
        //{
        //    PavilionStatusesSorting = GetPavilionStatusSorting();
        //}
        //#endregion


        //#region ActionEntities
        //private ActionEntities _currentActionEntities = ActionEntities.None;
        ///// <summary>
        ///// ActionEntities
        ///// </summary>
        //public ActionEntities CurrentActionEntities
        //{
        //    get => _currentActionEntities;
        //    set
        //    {
        //        Set(ref _currentActionEntities, value);

        //        if (_currentActionEntities == ActionEntities.Add)
        //        {
        //            CurrentPavilion = GetNewPavilion();
        //            ButtonName = "Добавить";
        //        }
        //        else if (_currentActionEntities == ActionEntities.Change)
        //        {
        //            ButtonName = "Изменить";
        //            CurrentPavilion = (
        //                from p in Manager.Instance.Context.Pavilion
        //                where p.mall_id == SelectedPavilion.mall_id &&
        //                        p.pavilion_number == SelectedPavilion.pavilion_number
        //                select p
        //            ).FirstOrDefault();
        //            SelectedPavilionStatus = SelectedPavilion.pavilion_status_name;
        //            SelectedMallPavilionName = SelectedPavilion.mall_name;
        //        }
        //    }
        //}
        //#endregion

        //#region Команда
        //public ICommand ExecuteCommand { get; }
        //private bool CanExecuteCommandExecute(object parameters) => true;
        //private void OnExecuteCommandExecuted(object parameters)
        //{
        //    if (CurrentPavilion.floor < 0 ||
        //        CurrentPavilion.square < 0 ||
        //        CurrentPavilion.value_added_factor < 0 ||
        //        CurrentPavilion.cost_per_square_meter < (decimal)0.1)
        //    {
        //        MessageBox.Show($"Числовые поля должны быть положительными.");
        //    }
        //    else if (CurrentPavilion.pavilion_number != null &&
        //             string.IsNullOrEmpty(CurrentPavilion.pavilion_number.Trim()) ||
        //             SelectedPavilionStatus == null)
        //    {
        //        MessageBox.Show($"Заполните все поля.");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            CurrentPavilion.status_id = (
        //                from ps in Manager.Instance.Context.Pavilion_statuses
        //                where ps.status_name == SelectedPavilionStatus
        //                select ps.status_id
        //            ).FirstOrDefault();
        //            CurrentPavilion.mall_id = (
        //                from m in Manager.Instance.Context.Mall
        //                where m.mall_name == SelectedMallPavilionName
        //                select m.mall_id
        //            ).FirstOrDefault();
        //            CurrentPavilion.pavilion_number = CurrentPavilion.pavilion_number.Trim();
        //            switch (CurrentActionEntities)
        //            {
        //                case ActionEntities.Add:
        //                    Manager.Instance.Context.Pavilion.Add(CurrentPavilion);
        //                    break;
        //                case ActionEntities.Change:
        //                    break;
        //            }
        //            Manager.Instance.Context.SaveChanges();
        //            switch (CurrentActionEntities)
        //            {
        //                case ActionEntities.Add:
        //                    MessageBox.Show($"Павильон добавлен.");
        //                    break;
        //                case ActionEntities.Change:
        //                    MessageBox.Show($"Павильон изменён.");
        //                    break;
        //            }
        //            Manager.Instance.MainFrameNavigate(new ViewingPavilionPage());
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show($"Ошибка :\n{e}");
        //        }
        //    }
        //    SelectedPavilionStatusSorting = AllNameSorting;
        //    UpdatePavilions();
        //}
        //#endregion

        //#region Статус сортировки Всё
        //private string _allNameSorting = "Всё";
        ///// <summary>
        ///// Статус сортировки Всё
        ///// </summary>
        //public string AllNameSorting
        //{
        //    get => _allNameSorting;
        //    set => Set(ref _allNameSorting, value);
        //}
        //#endregion

        //#region Статус сортировки Удалён
        //private string _deleteNameSorting = "Удалён";
        ///// <summary>
        ///// Статус сортировки Удалён
        ///// </summary>
        //public string DeleteNameSorting
        //{
        //    get => _deleteNameSorting;
        //    set => Set(ref _deleteNameSorting, value);
        //}
        //#endregion

        //#region Этаж
        //private int _floor = 0;
        ///// <summary>
        ///// Этаж
        ///// </summary>
        //public int Floor
        //{
        //    get => _floor;
        //    set
        //    {
        //        if (value >= 0)
        //        {
        //            Set(ref _floor, value);
        //            UpdatePavilions();
        //        }
        //    }
        //}
        //#endregion

        //#region Мин площадь
        //private decimal _minSquare = 0;
        ///// <summary>
        ///// Min площадь
        ///// </summary>
        //public decimal MinSquare
        //{
        //    get => _minSquare;
        //    set
        //    {
        //        if (value >= 0)
        //        {
        //            if (value <= MaxSquare)
        //            {
        //                Set(ref _minSquare, value);
        //            }
        //            else
        //            {
        //                Set(ref _minSquare, MaxSquare);
        //            }
        //            UpdatePavilions();
        //        }
        //    }
        //}
        //#endregion

        //#region Мин площадь
        //private decimal _maxSquare = 1000;
        ///// <summary>
        ///// Max площадь
        ///// </summary>
        //public decimal MaxSquare
        //{
        //    get => _maxSquare;
        //    set
        //    {
        //        if (value >= 0)
        //        {
        //            if (value >= MinSquare)
        //            {
        //                Set(ref _maxSquare, value);
        //            }
        //            else
        //            {
        //                Set(ref _maxSquare, MinSquare);
        //            }
        //            UpdatePavilions();
        //        }
        //    }
        //}
        //#endregion

        //#region Конструктор
        //public ViewingPavilionPageViewModel()
        //{
        //    AddPavilionCommand = new LambdaCommand(OnAddPavilionCommandExecuted, CanAddPavilionCommandExecute);
        //    ChangePavilionCommand = new LambdaCommand(OnChangePavilionCommandExecuted, CanChangePavilionCommandExecute);
        //    DeletePavilionCommand = new LambdaCommand(OnDeletePavilionCommandExecuted, CanDeletePavilionCommandExecute);
        //    ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecute);
        //    UpdatePavilionsCommand = new LambdaCommand(OnUpdatePavilionsCommandExecuted, CanUpdatePavilionsCommandExecute);

        //    UpdatePavilionStatuses();
        //    UpdatePavilionStatusSorting();
        //    // убираю сортировку по удаленным ТЦ
        //    PavilionStatusesSorting.Remove(DeleteNameSorting);
        //    // добавляю общую сортировку
        //    PavilionStatusesSorting.Add(AllNameSorting);
        //    // текущим выбранным ставим все
        //    SelectedPavilionStatusSorting = AllNameSorting;

        //    UpdatePavilions();
        //    UpdatePavilionNames();
        //    CurrentPavilion = GetNewPavilion();
        //}
        //#endregion
    }
}