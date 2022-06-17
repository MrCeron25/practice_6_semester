using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
                    //Debug.WriteLine(fileInfo);
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
        }
    }
}
