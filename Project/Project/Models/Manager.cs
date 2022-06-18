using System.Windows.Controls;

namespace Project
{
    public class Manager
    {
        private static Manager _INSTANCE;

        public static Manager Instance
        {
            get
            {
                if (_INSTANCE == null)
                {
                    _INSTANCE = new Manager();
                }
                return _INSTANCE;
            }
        }

        public Entities Context { get; set; }
        public Frame MainFrame { get; set; }
        public Frame MenuFrame { get; set; }
    }
}
