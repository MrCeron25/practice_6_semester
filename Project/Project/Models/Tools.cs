using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Project
{
    public class Tools
    {
        public static byte[] GetImageBytes(string filePath)
        {
            byte[] photo = null;
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        photo = reader.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show($"Ошибка :\n{e}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка :\n{e}");
            }
            return photo;
        }

        public static BitmapImage BytesToImage(byte[] bytes)
        {
            BitmapImage image = null;
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }
            }
            catch (ArgumentNullException e)
            {
                MessageBox.Show($"Ошибка загрузки фотографии.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка :\n{e}");
            }
            return image;
        }
    }
}
