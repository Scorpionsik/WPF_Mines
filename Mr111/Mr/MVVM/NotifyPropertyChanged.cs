using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mr.MVVM
{
    /// <summary>
    /// Реализует интерфейс <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        // <summary>
        /// Событие для обновления привязанного объекта (в XAML)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод для обновления выбранного привязанного объекта (в XAML)
        /// </summary>
        /// <param name="prop">Принимает строку-имя объекта, который необходимо обновить</param>
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //---метод OnPropertyChanged
    } //---класс NotifyPropertyChanged
} //---пространство имён CoreWPF.MVVM
//---EOF