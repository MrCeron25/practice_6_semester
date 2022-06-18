using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Project.ViewModels.Base
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            // CallerMemberName - компилятор автоматом подставит имя метода из которого вызывается метод
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Обновление значения свойства
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">ссылка на поле свойства</param>
        /// <param name="value">новое значение</param>
        /// <param name="">Имя свойства</param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
