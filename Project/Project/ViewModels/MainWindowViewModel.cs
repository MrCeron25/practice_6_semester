using Project.ViewModels.Base;
using System;
using System.Windows.Media.Imaging;

namespace Project.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок
        private string _tiile = "KingIT";
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title
        {
            get => _tiile;
            set => Set(ref _tiile, value);
        }
        #endregion

        #region Image
        private BitmapImage _iconImage = Tools.BytesToImage(Tools.GetImageBytes(@"..\..\Images\Эмблема.png"));
        /// <summary>
        /// Image
        /// </summary>
        public BitmapImage IconImage
        {
            get => _iconImage;
            set => Set(ref _iconImage, value);
        }
        #endregion
    }
}
