using Project.ViewModels.Base;

namespace Project.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок
        private string _tiile = "Заголовок";
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title
        {
            get => _tiile;
            set => Set(ref _tiile, value);
        }
        #endregion
    }
}
