using System.IO;
using System.Windows.Media.Imaging;

namespace Project
{
    public class Tools
    {
        public static byte[] GetImageBytes(string filePath)
        {
            byte[] photo = null;
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    photo = reader.ReadBytes((int)stream.Length);
                }
            }
            return photo;
        }

        public static BitmapImage BytesToImage(byte[] bytes)
        {
            BitmapImage image = null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
            }
            return image;
        }
    }
}
