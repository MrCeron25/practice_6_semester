using System.Windows.Input;
using Project.ViewModels.Base;
using System.Linq;
using Project.Infrastructure.Commands;
using System.Windows;
using System.Threading;

namespace Project.ViewModels
{
    internal class MainMenuViewModel : ViewModel
    {
        #region Вход
        private string _entryButtonName = "Вход";
        /// <summary>
        /// Вход
        /// </summary>
        public string EntryButtonName
        {
            get => _entryButtonName;
            set => Set(ref _entryButtonName, value);
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

        #region Закрытие приложения
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object parameters) => true;
        private void OnCloseApplicationCommandExecuted(object parameters)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Вход
        public ICommand EntryCommand { get; }
        private bool CanEntryCommandExecute(object parameters) => true;
        private void OnEntryCommandExecuted(object parameters)
        {
            Manager.Instance.MainFrame.Navigate(new LoginPage());
        }
        #endregion

        #region Тестовое подключение
        /// <summary>
        /// Для ускорения загрузки данных для EF
        /// </summary>
        private void TestConnect()
        {
            var data = from em in Manager.Instance.Context.Employees
                       select em;
        }
        #endregion

        #region Конструктор
        public MainMenuViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            EntryCommand = new LambdaCommand(OnEntryCommandExecuted, CanEntryCommandExecute);
            Thread testConnect = new Thread(TestConnect);
            testConnect.Start();
        }
        #endregion
    }
}
