using Project.Infrastructure.Commands;
using Project.ViewModels.Base;
using Project.Views.Pages;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class ManagerCMenuPageViewModel : ViewModel
    {
        #region ТЦ
        private string _mallButtonName = "ТЦ";
        /// <summary>
        /// ТЦ
        /// </summary>
        public string MallButtonName
        {
            get => _mallButtonName;
            set => Set(ref _mallButtonName, value);
        }
        #endregion

        #region Пользователи
        private string _employeesButtonName = "Пользователи";
        /// <summary>
        /// Пользователи
        /// </summary>
        public string EmployeesButtonName
        {
            get => _employeesButtonName;
            set => Set(ref _employeesButtonName, value);
        }
        #endregion

        #region Выход
        private string _exitButtonName = "Выход";
        /// <summary>
        /// Выход
        /// </summary>
        public string ExitButtonName
        {
            get => _exitButtonName;
            set => Set(ref _exitButtonName, value);
        }
        #endregion
        
        #region Команда перехода на страницу ТЦ
        public ICommand GoMallCommand { get; }
        private bool CanGoMallCommandExecute(object parameters) => true;
        private void OnGoMallCommandExecuted(object parameters)
        {
            Manager.Instance.MainFrameNavigate(new ViewingMallPage());
        }
        #endregion

        #region Команда перехода на страницу ТЦ
        public ICommand GoEmployeeCommand { get; }
        private bool CanGoEmployeeCommandExecute(object parameters) => true;
        private void OnGoEmployeeCommandExecuted(object parameters)
        {
            //Manager.Instance.MainFrameNavigate(new ViewingMallPage());
        }
        #endregion

        #region Команда выхода
        public ICommand ExitCommand { get; }
        private bool CanExitCommandExecute(object p) => true;
        private void OnExitCommandExecuted(object p)
        {
            Manager.Instance.LoadEmployeeInterface(new Models.Employees { role = 0 });
        }
        #endregion

        #region Конструктор
        public ManagerCMenuPageViewModel()
        {
            GoMallCommand = new LambdaCommand(OnGoMallCommandExecuted, CanGoMallCommandExecute);
            ExitCommand = new LambdaCommand(OnExitCommandExecuted, CanExitCommandExecute);
            GoEmployeeCommand = new LambdaCommand(OnGoEmployeeCommandExecuted, CanGoEmployeeCommandExecute);
        }
        #endregion
    }
}
