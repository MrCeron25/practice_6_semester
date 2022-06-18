using Project.ViewModels.Base;

namespace Project.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок
        private string _Tiile = "Заголовок";

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title
        {
            get => _Tiile;
            set => Set(ref _Tiile, value);
        }

        #endregion
    }
}
