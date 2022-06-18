using Project.Infrastructure.Commands;
using Project.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class LoginPageViewModel : ViewModel
    {
        #region Логин
        private string _LoginLabelName = "Логин";

        /// <summary>
        /// Логин
        /// </summary>
        public string LoginLabelName
        {
            get => _LoginLabelName;
            set => Set(ref _LoginLabelName, value);
        }
        #endregion

        #region Регистрация
        private string _RegistrationButtonName = "Регистрация";

        /// <summary>
        /// Регистрация
        /// </summary>
        public string LoginText
        {
            get => _RegistrationButtonName;
            set => Set(ref _RegistrationButtonName, value);
        }

        #endregion

        #region Выход
        private string _ExitButtonName = "Выход";

        /// <summary>
        /// Выход
        /// </summary>
        public string ExitButtonName
        {
            get => _ExitButtonName;
            set => Set(ref _ExitButtonName, value);
        }

        #endregion

        #region Команды

        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object parameters) => true;

        private void OnCloseApplicationCommandExecuted(object parameters)
        {
            Application.Current.Shutdown();
        }

        public ICommand EntryCommand { get; }

        private bool CanEntryCommandExecute(object parameters) => true;

        private void OnEntryCommandExecuted(object parameters)
        {
            Manager.Instance.MainFrame.Navigate(new LoginPage());
        }

        public ICommand RegistrationCommand { get; }

        private bool CanRegistrationCommandExecute(object p) => true;

        private void OnRegistrationCommandExecuted(object p)
        {
            //Manager.Instance.MainFrame.Navigate(new RegistrationPage());
        }
        #endregion

        #region Конструктор
        public LoginPageViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            EntryCommand = new LambdaCommand(OnEntryCommandExecuted, CanEntryCommandExecute);
            RegistrationCommand = new LambdaCommand(OnRegistrationCommandExecuted, CanRegistrationCommandExecute);
        }
        #endregion
    }
}
