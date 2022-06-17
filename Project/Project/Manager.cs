using System.Windows.Controls;

namespace Project
{
    public class Manager
    {
        private static Frame _Instance = null;
        public static Frame MainFrame
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Frame();
                }
                return _Instance;
            }
            set => _Instance = value;
        }
    }
}
