using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Project
{
    public partial class LoginPage : Page
    {
        private int Count = 0;
        private string captchaText = string.Empty;
        public LoginPage()
        {
            InitializeComponent();
            UpdateImage();
        }

        private void UpdateImage()
        {
            CaptchaImage.Source = ImageSourceFromBitmap(CreateImage((int)CaptchaImage.Width, (int)CaptchaImage.Height));
        }

        public System.Windows.Media.ImageSource ImageSourceFromBitmap(Bitmap bmp)
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

        private void EntryClick()
        {
            using (Entities cnt = new Entities())
            {
                List<Employees> Employees_ = (from em in cnt.Employees
                                              where em.login.ToLower() == Login.Text.ToLower() && em.password == Password.Password
                                              select em).ToList();
                if (Employees_.Count == 0)
                {
                    MessageBox.Show("Пользователь не найден.");
                    Count++;
                }
                else
                {
                    Debug.WriteLine("Пользователь найден.");
                    Employees em = Employees_[0];
                    Manager.MainFrame.Navigate(new ManagerPage());
                }
                if (Count == 3)
                {
                    CaptchaImage.Visibility = Visibility.Visible;
                    CaptchaText.Visibility = Visibility.Visible;
                    UpdateCaptcha.Visibility = Visibility.Visible;
                }
            }
        }

        private void Entry_Click(object sender, RoutedEventArgs e)
        {
            if (Count != 3)
            {
                EntryClick();
            }
            else if (CaptchaText.Text.ToLower() == captchaText.ToLower())
            {
                EntryClick();
                CaptchaImage.Visibility = Visibility.Hidden;
                CaptchaText.Visibility = Visibility.Hidden;
                UpdateCaptcha.Visibility = Visibility.Hidden;
                Count = 0;
                CaptchaText.Text = string.Empty;
                UpdateImage();
            }
            else
            {
                MessageBox.Show("Неверная капча.");
            }
        }

        private void LoadEmployeesImages_Click(object sender, RoutedEventArgs e)
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

        private void LoadMallImages_Click(object sender, RoutedEventArgs e)
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

        #region 

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
            captchaText = string.Empty;
            string ALF = "7890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
            {
                captchaText += ALF[rnd.Next(ALF.Length)];
            }

            //Нарисуем сгенирируемый текст
            g.DrawString(captchaText,
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
        #endregion

        private void TextBlock_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdateImage();
        }
    }
}
