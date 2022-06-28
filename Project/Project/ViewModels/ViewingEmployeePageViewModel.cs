using Project.Infrastructure.Commands;
using Project.ViewModels.Base;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Project.Models;
using Project.Views.Pages;
using System.Windows;
using System;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Project.ViewModels
{
    internal class ViewingEmployeePageViewModel : ViewModel
    {
        #region Сотрудники
        private ObservableCollection<EmployeeItem> _employees;
        /// <summary>
        /// Сотрудники
        /// </summary>
        public ObservableCollection<EmployeeItem> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        #endregion

        #region Выбранный cотрудник
        private EmployeeItem _selectedEmployee;
        /// <summary>
        /// Выбранный cотрудник
        /// </summary>
        public EmployeeItem SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                IsSelectedEmployee = value != null;
                Set(ref _selectedEmployee, value);
            }
        }
        #endregion

        #region Cотрудник выбран
        private bool _isSelectedEmployee = false;
        /// <summary>
        /// Cотрудник выбран
        /// </summary>
        public bool IsSelectedEmployee
        {
            get => _isSelectedEmployee;
            set => Set(ref _isSelectedEmployee, value);
        }
        #endregion

        #region Сотрудники заголовок
        private string _employeesTitleName = "Сотрудники";
        /// <summary>
        /// Сотрудники заголовок
        /// </summary>
        public string EmployeesTitleName
        {
            get => _employeesTitleName;
            set => Set(ref _employeesTitleName, value);
        }
        #endregion

        #region UpdateEmployees
        private void UpdateEmployees()
        {
            Employees = GetEmployees();
        }
        #endregion



        #region DeletedStatus
        private string _deletedStatus = "Удалён";
        /// <summary>
        /// DeletedStatus
        /// </summary>
        public string DeletedStatus
        {
            get => _deletedStatus;
            set => Set(ref _deletedStatus, value);
        }
        #endregion


        #region GetEmployees
        private ObservableCollection<EmployeeItem> GetEmployees()
        {
            return new ObservableCollection<EmployeeItem>(
                from e in Manager.Instance.Context.Employees
                join er in Manager.Instance.Context.Employees_roles on e.role equals er.role_id
                orderby e.surname, e.name, e.patronymic
                where er.role_name != DeletedStatus
                select new EmployeeItem
                {
                    employe_id = e.employe_id,
                    name = e.name,
                    surname = e.surname,
                    patronymic = e.patronymic,
                    login = e.login,
                    password = e.password,
                    role = e.role,
                    role_name = er.role_name,
                    phone = e.phone,
                    sex = e.sex,
                    photo = e.photo
                }
            );
        }
        #endregion

        #region Добавить сотрудника
        public ICommand AddEmployeeCommand { get; }
        private bool CanAddEmployeeCommandExecute(object parameters) => true;
        private void OnAddEmployeeCommandExecuted(object parameters)
        {
            CurrentActionEntities = ActionEntities.Add;
            Manager.Instance.MainFrameNavigate(new EmployeePage());
        }
        #endregion

        #region Изменить сотрудника
        public ICommand ChangeEmployeeCommand { get; }
        private bool CanChangeEmployeeCommandExecute(object parameters) => IsSelectedEmployee;
        private void OnChangeEmployeeCommandExecuted(object parameters)
        {
            CurrentActionEntities = ActionEntities.Change;
            Manager.Instance.MainFrameNavigate(new EmployeePage());
        }
        #endregion

        #region Удалить сотрудника
        public ICommand DeleteEmployeeCommand { get; }
        private bool CanDeleteEmployeeCommandExecute(object parameters) => IsSelectedEmployee;
        private void OnDeleteEmployeeCommandExecuted(object parameters)
        {
            try
            {
                Employees employee = (
                    from em in Manager.Instance.Context.Employees
                    where em.employe_id == SelectedEmployee.employe_id
                    select em
                ).FirstOrDefault();
                employee.role = (
                    from er in Manager.Instance.Context.Employees_roles
                    where er.role_name == DeletedStatus
                    select er.role_id
                ).FirstOrDefault();
                Manager.Instance.Context.SaveChanges();
                Employees.Remove(SelectedEmployee);
                MessageBox.Show($"Сотрудник удалён.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка :\n{e}");
            }
        }
        #endregion

        #region EmployeeSex
        private ObservableCollection<string> _employeeSex = new ObservableCollection<string>()
        {
            "М",
            "Ж"
        };
        /// <summary>
        /// EmployeeSex
        /// </summary>
        public ObservableCollection<string> EmployeeSex
        {
            get => _employeeSex;
            set => Set(ref _employeeSex, value);
        }
        #endregion

        #region EmployeeSex
        private string _selectedEmployeeSex;
        /// <summary>
        /// EmployeeSex
        /// </summary>
        public string SelectedEmployeeSex
        {
            get => _selectedEmployeeSex;
            set => Set(ref _selectedEmployeeSex, value);
        }
        #endregion

        #region EmployeeRoles
        private ObservableCollection<string> _employeeRoles;
        /// <summary>
        /// EmployeeRoles
        /// </summary>
        public ObservableCollection<string> EmployeeRoles
        {
            get => _employeeRoles;
            set => Set(ref _employeeRoles, value);
        }
        #endregion

        #region SelectedEmployeeRoles
        private string _selectedEmployeeRoles;
        /// <summary>
        /// SelectedEmployeeRoles
        /// </summary>
        public string SelectedEmployeeRoles
        {
            get => _selectedEmployeeRoles;
            set => Set(ref _selectedEmployeeRoles, value);
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
                        CurrentEmployee.photo = Tools.GetImageBytes(fileDialog.FileName);
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


        #region EmployeeButtonName
        private string _employeeButtonName;
        /// <summary>
        /// EmployeeButtonName
        /// </summary>
        public string EmployeeButtonName
        {
            get => _employeeButtonName;
            set => Set(ref _employeeButtonName, value);
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
                    CurrentEmployee = new Employees();
                    EmployeeButtonName = "Добавить";
                    LoadedImage = null;
                }
                else if (_currentActionEntities == ActionEntities.Change)
                {
                    EmployeeButtonName = "Изменить";
                    CurrentEmployee = (
                        from em in Manager.Instance.Context.Employees
                        where em.employe_id == SelectedEmployee.employe_id
                        select em
                    ).FirstOrDefault();
                    SelectedEmployeeSex = CurrentEmployee.sex;
                    SelectedEmployeeRoles = (
                        from emr in Manager.Instance.Context.Employees_roles
                        where emr.role_id == CurrentEmployee.role
                        select emr.role_name
                    ).FirstOrDefault();
                    if (CurrentEmployee.photo != null)
                    {
                        LoadedImage = Tools.BytesToImage(CurrentEmployee.photo);
                    }
                }
            }
        }
        #endregion

        #region Текущий сотрудник
        private Employees _сurrentEmployee;
        /// <summary>
        /// Текущий сотрудник
        /// </summary>
        public Employees CurrentEmployee
        {
            get => _сurrentEmployee;
            set => Set(ref _сurrentEmployee, value);
        }
        #endregion

        #region Команда
        public ICommand ExecuteCommand { get; }
        private bool CanExecuteCommandExecute(object parameters) => true;
        private void OnExecuteCommandExecuted(object parameters)
        {
            if (string.IsNullOrEmpty(CurrentEmployee.surname.Trim()) ||
                string.IsNullOrEmpty(CurrentEmployee.name.Trim()) ||
                string.IsNullOrEmpty(CurrentEmployee.patronymic.Trim()) ||
                string.IsNullOrEmpty(CurrentEmployee.login.Trim()) ||
                string.IsNullOrEmpty(CurrentEmployee.password.Trim()) ||
                string.IsNullOrEmpty(CurrentEmployee.phone.Trim()) ||
                LoadedImage == null ||
                SelectedEmployeeSex == null ||
                SelectedEmployeeRoles == null)
            {
                MessageBox.Show($"Заполните все поля.");
            }
            else
            {
                try
                {
                    CurrentEmployee.surname = CurrentEmployee.surname.Trim();
                    CurrentEmployee.name = CurrentEmployee.name.Trim();
                    CurrentEmployee.patronymic = CurrentEmployee.patronymic.Trim();
                    CurrentEmployee.login = CurrentEmployee.login.Trim();
                    CurrentEmployee.password = CurrentEmployee.password.Trim();
                    CurrentEmployee.phone = CurrentEmployee.phone.Trim();
                    CurrentEmployee.sex = SelectedEmployeeSex;
                    CurrentEmployee.role = (
                        from em in Manager.Instance.Context.Employees_roles
                        where em.role_name == SelectedEmployeeRoles
                        select em.role_id
                    ).FirstOrDefault();
                    switch (CurrentActionEntities)
                    {
                        case ActionEntities.Add:
                            Manager.Instance.Context.Employees.Add(CurrentEmployee);
                            break;
                        case ActionEntities.Change:
                            break;
                    }
                    Manager.Instance.Context.SaveChanges();
                    switch (CurrentActionEntities)
                    {
                        case ActionEntities.Add:
                            MessageBox.Show($"Сотрудник добавлен.");
                            break;
                        case ActionEntities.Change:
                            MessageBox.Show($"Сотрудник изменён.");
                            break;
                    }
                    Manager.Instance.MainFrameNavigate(new ViewingEmployeePage());
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Ошибка :\n{e}");
                }
            }
            UpdateEmployees();
        }
        #endregion


        #region GetEmployeeRoles
        private ObservableCollection<string> GetEmployeeRoles()
        {
            return new ObservableCollection<string>(
                from em in Manager.Instance.Context.Employees_roles
                select em.role_name
            );
        }
        #endregion

        #region UpdateEmployeeRoles
        private void UpdateEmployeeRoles()
        {
            EmployeeRoles = GetEmployeeRoles();
        }
        #endregion


        #region Конструктор
        public ViewingEmployeePageViewModel()
        {

            AddEmployeeCommand = new LambdaCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);
            ChangeEmployeeCommand = new LambdaCommand(OnChangeEmployeeCommandExecuted, CanChangeEmployeeCommandExecute);
            DeleteEmployeeCommand = new LambdaCommand(OnDeleteEmployeeCommandExecuted, CanDeleteEmployeeCommandExecute);

            LoadPhotoCommand = new LambdaCommand(OnLoadPhotoCommandExecuted, CanLoadPhotoCommandExecute);

            ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecute);

            SelectedEmployeeSex = EmployeeSex[0];

            UpdateEmployees();
            UpdateEmployeeRoles();


            //DeleteMallCommand = new LambdaCommand(OnDeleteMallCommandExecuted, CanDeleteMallCommandExecute);
            //AddMallCommand = new LambdaCommand(OnAddMallCommandExecuted, CanAddMallCommandExecute);
            //ChangeMallCommand = new LambdaCommand(OnChangeMallCommandExecuted, CanChangeMallCommandExecute);
            //ViewMallCommand = new LambdaCommand(OnViewMallCommandExecuted, CanViewMallCommandExecute);
            //UpdateMalls();

            //MallExecuteCommand = new LambdaCommand(OnMallExecuteCommandExecuted, CanMallExecuteCommandExecute);
            //LoadPhotoCommand = new LambdaCommand(OnLoadPhotoCommandExecuted, CanLoadPhotoCommandExecute);
            //UpdateMallStatuses();

            //MallStatusesSorting = GetMallStatuses();

            //// убираю сортировку по удаленным ТЦ
            //MallStatusesSorting.Remove(DeleteNameSorting);
            //// добавляю общую сортировку
            //MallStatusesSorting.Add(AllNameSorting);
            //// текущим выбранным ставим все
            //SelectedMallStatusSorting = AllNameSorting;

            //UpdateCities();
            ////// текущим выбранным ставим все
            //SelectedCity = AllNameSorting;

            //#region ViewingPavilionPageViewModel

            //AddPavilionCommand = new LambdaCommand(OnAddPavilionCommandExecuted, CanAddPavilionCommandExecute);
            //ChangePavilionCommand = new LambdaCommand(OnChangePavilionCommandExecuted, CanChangePavilionCommandExecute);
            //DeletePavilionCommand = new LambdaCommand(OnDeletePavilionCommandExecuted, CanDeletePavilionCommandExecute);
            //ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecute);
            //UpdatePavilionsCommand = new LambdaCommand(OnUpdatePavilionsCommandExecuted, CanUpdatePavilionsCommandExecute);

            //UpdatePavilionStatuses();
            //UpdatePavilionStatusSorting();
            //// убираю сортировку по удаленным ТЦ
            //PavilionStatusesSorting.Remove(DeleteNameSorting);
            //// добавляю общую сортировку
            //PavilionStatusesSorting.Add(AllNameSorting);
            //// текущим выбранным ставим все
            //SelectedPavilionStatusSorting = AllNameSorting;

            //UpdatePavilions();
            //UpdatePavilionNames();
            //CurrentPavilion = GetNewPavilion();
            //#endregion

            //#region PavilionRentalPage

            //RentPavilionCommand = new LambdaCommand(OnRentPavilionCommandExecuted, CanRentPavilionCommandExecute);
            //PavilionRentalCommand = new LambdaCommand(OnPavilionRentalCommandExecuted, CanPavilionRentalCommandExecute);
            //UpdateRentActions();
            //UpdateTenants();
            //UpdateEmployees();
            //SelectedRentAction = RentActionName;

            //RentalEndDate = DateTime.Today.AddDays(1);

            //#endregion
        }
        #endregion

    }
}
