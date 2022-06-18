using Project.Infrastructure.Commands;
using Project.ViewModels.Base;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Controls;

namespace Project.ViewModels
{
    internal class LoginPageViewModel : ViewModel
    {
        #region Логин
        private string _loginLabelName = "Логин";
        /// <summary>
        /// Логин
        /// </summary>
        public string LoginLabelName
        {
            get => _loginLabelName;
            set => Set(ref _loginLabelName, value);
        }
        #endregion

        #region Заголовок пароля
        private string _passwordLabelName = "Пароль";
        /// <summary>
        /// Заголовок пароля
        /// </summary>
        public string PasswordLabelName
        {
            get => _passwordLabelName;
            set => Set(ref _passwordLabelName, value);
        }
        #endregion

        #region Текст кнопки входа 
        private string _exitButtonText = "Вход";
        /// <summary>
        /// Текст кнопки входа 
        /// </summary>
        public string EntryButtonText
        {
            get => _exitButtonText;
            set => Set(ref _exitButtonText, value);
        }
        #endregion


        #region Текст логина
        private string _loginText = "Adam@gmail.com";
        /// <summary>
        /// Текст логина
        /// </summary>
        public string LoginText
        {
            get => _loginText;
            set => Set(ref _loginText, value);
        }
        #endregion

        #region Видимость кнопки загрузки фотографий сотрудников
        private Visibility _loadEmployeesImagesVisibility = Visibility.Visible;
        /// <summary>
        /// Видимость кнопки загрузки фото сотрудников
        /// </summary>
        public Visibility LoadEmployeesImagesVisibility
        {
            get => _loadEmployeesImagesVisibility;
            set => Set(ref _loadEmployeesImagesVisibility, value);
        }
        #endregion

        #region Название кнопки загрузки фото сотрудников
        private string _employeesImagesButtonText = "Загрузить фотографии сотрудников";
        /// <summary>
        /// Название кнопки загрузки фото сотрудников
        /// </summary>
        public string EmployeesImagesButtonText
        {
            get => _employeesImagesButtonText;
            set => Set(ref _employeesImagesButtonText, value);
        }
        #endregion

        #region Видимость кнопки загрузки фотографий ТЦ
        private Visibility _loadMallImagesVisibility = Visibility.Visible;
        /// <summary>
        /// Видимость кнопки загрузки фото ТЦ
        /// </summary>
        public Visibility LoadMallImagesVisibility
        {
            get => _loadMallImagesVisibility;
            set => Set(ref _loadMallImagesVisibility, value);
        }
        #endregion

        #region Название кнопки загрузки фото ТЦ
        private string _loadMallImagesButtonText = "Загрузить фотографии ТЦ";
        /// <summary>
        /// Название кнопки загрузки фото ТЦ
        /// </summary>
        public string LoadMallImagesButtonText
        {
            get => _loadMallImagesButtonText;
            set => Set(ref _loadMallImagesButtonText, value);
        }
        #endregion

        #region Название текста изминения капчи
        private string _updateCaptchaText = "Изменить картинку";
        /// <summary>
        /// Название кнопки загрузки фото ТЦ
        /// </summary>
        public string UpdateCaptchaText
        {
            get => _updateCaptchaText;
            set => Set(ref _updateCaptchaText, value);
        }
        #endregion


        #region Видимость поля ввода капчи
        private Visibility _сaptchaTextBoxVisibility = Visibility.Hidden;
        /// <summary>
        /// Видимость поля ввода капчи
        /// </summary>
        public Visibility CaptchaTextBoxVisibility
        {
            get => _сaptchaTextBoxVisibility;
            set => Set(ref _сaptchaTextBoxVisibility, value);
        }
        #endregion


        #region Видимость текста изминения капчи
        private Visibility _updateCaptchaTextBlockVisibility = Visibility.Hidden;
        /// <summary>
        /// Видимость текста изминения капчи
        /// </summary>
        public Visibility UpdateCaptchaTextBlockVisibility
        {
            get => _updateCaptchaTextBlockVisibility;
            set => Set(ref _updateCaptchaTextBlockVisibility, value);
        }
        #endregion


        #region Видимость текста изминения капчи
        private Visibility _captchaTextVisibility = Visibility.Hidden;
        /// <summary>
        /// Видимость текста изминения капчи
        /// </summary>
        public Visibility CaptchaTextVisibility
        {
            get => _captchaTextVisibility;
            set => Set(ref _captchaTextVisibility, value);
        }
        #endregion

        #region Видимость капчи
        private Visibility _captchaImageVisibility = Visibility.Hidden;
        /// <summary>
        /// Видимость капчи
        /// </summary>
        public Visibility CaptchaImageVisibility
        {
            get => _captchaImageVisibility;
            set => Set(ref _captchaImageVisibility, value);
        }
        #endregion

        #region Капча
        private System.Windows.Media.ImageSource _captchaImage = null;
        /// <summary>
        /// Капча
        /// </summary>
        public System.Windows.Media.ImageSource CaptchaImage
        {
            get => _captchaImage;
            set => Set(ref _captchaImage, value);
        }
        #endregion

        #region Текст капчи
        private string _captchaText = string.Empty;
        /// <summary>
        /// Текст капчи
        /// </summary>
        public string CaptchaText
        {
            get => _captchaText;
            set => Set(ref _captchaText, value);
        }
        #endregion

        #region Текст сгенерированной капчи
        private string _generatedCaptchaText = string.Empty;
        /// <summary>
        /// Текст сгенерированной капчи
        /// </summary>
        public string GeneratedCaptchaText
        {
            get => _generatedCaptchaText;
            set => Set(ref _generatedCaptchaText, value);
        }
        #endregion

        #region Счетчик неправильных попыток
        private int _count = 0;
        /// <summary>
        /// Счетчик неправильных попыток
        /// </summary>
        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }
        #endregion

        private bool EntryClick(string Password)
        {
            bool result = false;
            List<Employees> Employees_ = (from em in Manager.Instance.Context.Employees
                                          where em.login.ToLower() == LoginText.ToLower() && em.password == Password
                                          select em).ToList();
            if (Employees_.Count == 0)
            {
                MessageBox.Show("Пользователь не найден.");
                Count++;
            }
            else
            {
                result = true;
                Debug.WriteLine("Пользователь найден.");
                Employees em = Employees_[0];
                switch (em.role)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:
                        Manager.Instance.MainFrameNavigate(new ManagerCPage());
                        break;
                    case 4:

                        break;
                    default:

                        break;
                }
            }
            if (Count == 3)
            {
                CaptchaImageVisibility = Visibility.Visible;
                CaptchaTextBoxVisibility = Visibility.Visible;
                UpdateCaptchaTextBlockVisibility = Visibility.Visible;
            }
            return result;
        }

        private void UpdateImage(int Width = 250, int Height = 60)
        {
            CaptchaImage = BitmapToImageSource(CreateImage(Width, Height));
        }

        public System.Windows.Media.ImageSource BitmapToImageSource(Bitmap bmp)
        {
            try
            {
                IntPtr handle;
                handle = bmp.GetHbitmap();
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                return null;
            }
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();
            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);
            //Вычислим позицию текста
            int Xpos = 10;
            int Ypos = 10;
            //Добавим различные цвета ддя текста
            Brush[] colors = {
                Brushes.Black,
                Brushes.Red,
                Brushes.RoyalBlue,
                Brushes.Green,
                Brushes.Yellow,
                Brushes.White,
                Brushes.Tomato,
                Brushes.Sienna,
                Brushes.Pink };
            //Добавим различные цвета линий
            Pen[] colorpens = {
                Pens.Black,
                Pens.Red,
                Pens.RoyalBlue,
                Pens.Green,
                Pens.Yellow,
                Pens.White,
                Pens.Tomato,
                Pens.Sienna,
                Pens.Pink };
            //Делаем случайный стиль текста
            System.Drawing.FontStyle[] fontstyle = {
                System.Drawing.FontStyle.Bold,
                System.Drawing.FontStyle.Italic,
                System.Drawing.FontStyle.Regular,
                System.Drawing.FontStyle.Strikeout,
                System.Drawing.FontStyle.Underline};

            //Добавим различные углы поворота текста
            short[] rotate = { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5, 6, -6 };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage(result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Делаем случайный угол поворота текста
            g.RotateTransform(rnd.Next(rotate.Length));

            //Генерируем текст
            GeneratedCaptchaText = string.Empty;
            string symbols = string.Empty;
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            symbols += alphabet;
            symbols += alphabet.ToLower();
            symbols += "0123456789";
            for (int i = 0; i < 6; ++i)
            {
                GeneratedCaptchaText += symbols[rnd.Next(symbols.Length)];
            }

            //Нарисуем сгенирируемый текст
            g.DrawString(GeneratedCaptchaText,
                new Font("Arial", 25, fontstyle[rnd.Next(fontstyle.Length)]),
                colors[rnd.Next(colors.Length)],
                new PointF(Xpos, Ypos));

            //Добавим немного помех
            //Линии из углов
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
                new System.Drawing.Point(0, 0),
                new System.Drawing.Point(Width - 1, Height - 1));
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
                new System.Drawing.Point(0, Height - 1),
                new System.Drawing.Point(Width - 1, 0));

            //Белые точки
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    if (rnd.Next() % 20 == 0)
                    {
                        result.SetPixel(i, j, Color.White);
                    }
                }
            }
            return result;
        }


        #region Команды
        public ICommand LoadEmployeesImagesCommand { get; }

        private bool CanLoadEmployeesImagesCommandExecute(object parameters) => true;

        private void OnLoadEmployeesImagesCommandExecuted(object parameters)
        {
            string pathToDirWithImages = $@"C:\Users\ARTEM\Desktop\КОРОНОВИРУС\21-22\Практика\Ресурсы\Image Сотрудники";

            DirectoryInfo directoryInfo = new DirectoryInfo(pathToDirWithImages);

            List<string> employeesImages = new List<string>();

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension == ".jpg")
                {
                    Debug.WriteLine(fileInfo);
                    employeesImages.Add(fileInfo.FullName);
                }
            }
            int index = 0;
            using (Entities context = new Entities())
            {
                foreach (Employees employee in context.Employees)
                {
                    employee.photo = Tools.GetImageBytes(employeesImages[index++]);
                    if (index == employeesImages.Count)
                    {
                        index = 0;
                    }
                }
                context.SaveChanges();
            }
            Debug.WriteLine("Фото сотрудников загружены.");
        }

        public ICommand LoadMallImagesCommand { get; }

        private bool CanLoadMallImagesCommandExecute(object parameters) => true;

        private void OnLoadMallImagesCommandExecuted(object parameters)
        {
            string pathToDirWithImages = $@"C:\Users\ARTEM\Desktop\КОРОНОВИРУС\21-22\Практика\Ресурсы\Image ТЦ";

            DirectoryInfo directoryInfo = new DirectoryInfo(pathToDirWithImages);

            List<string> mallImages = new List<string>();

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension == ".jpg")
                {
                    Debug.WriteLine(fileInfo);
                    mallImages.Add(fileInfo.FullName);
                }
            }
            int index = 0;
            using (Entities context = new Entities())
            {
                foreach (Mall mall in context.Mall)
                {
                    mall.photo = Tools.GetImageBytes(mallImages[index++]);
                    if (index == mallImages.Count)
                    {
                        index = 0;
                    }
                }
                context.SaveChanges();
            }
            Debug.WriteLine("Фото ТЦ загружены.");
        }

        public ICommand EntryCommand { get; }

        private bool CanEntryCommandExecute(object p) => true;

        private void OnEntryCommandExecuted(object p)
        {
            PasswordBox passwordBox = p as PasswordBox;
            string password = passwordBox.Password;
            if (Count != 3)
            {
                EntryClick(password);
            }
            else if (CaptchaText == GeneratedCaptchaText)
            {
                if (EntryClick(password))
                {
                    CaptchaImageVisibility = Visibility.Hidden;
                    CaptchaTextBoxVisibility = Visibility.Hidden;
                    UpdateCaptchaTextBlockVisibility = Visibility.Hidden;
                    Count = 0;
                    CaptchaText = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Неверная капча.");
            }
            UpdateImage();
        }

        public ICommand UpdateCaptchaLinkCommand { get; }

        private bool CanUpdateCaptchaLinkCommandExecute(object p) => true;

        private void OnUpdateCaptchaLinkCommandExecuted(object p)
        {
            UpdateImage();
        }

        #endregion

        #region Конструктор
        public LoginPageViewModel()
        {
            //LoadEmployeesImagesCommand = LambdaCommand
            //    .Builder()
            //    .CanExecute(CanLoadEmployeesImagesCommandExecute)
            //    .Execute(OnLoadEmployeesImagesCommandExecuted)
            //    .Build();
            LoadEmployeesImagesCommand = new LambdaCommand(OnLoadEmployeesImagesCommandExecuted, CanLoadEmployeesImagesCommandExecute);
            LoadMallImagesCommand = new LambdaCommand(OnLoadMallImagesCommandExecuted, CanLoadMallImagesCommandExecute);
            EntryCommand = new LambdaCommand(OnEntryCommandExecuted, CanEntryCommandExecute);
            UpdateCaptchaLinkCommand = new LambdaCommand(OnUpdateCaptchaLinkCommandExecuted, CanUpdateCaptchaLinkCommandExecute);
            UpdateImage();
        }
        #endregion
    }
}
