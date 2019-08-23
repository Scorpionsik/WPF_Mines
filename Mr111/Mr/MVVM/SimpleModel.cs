using System;
using System.Linq;

namespace Mr.MVVM
{
    public abstract partial class SimpleModel : NotifyPropertyChanged
    {
        #region Поля и свойства
        private event Action<SimpleModel> event_select_model;
        /// <summary>
        /// Событие выбора данной модели
        /// </summary>
        public Action<SimpleModel> Event_select_model
        {
            get { return this.event_select_model; }
            set
            {
                this.event_select_model = new Action<SimpleModel>(value);
            }
        } //---свойство Event_select_model

        /// <summary>
        /// Возвращает имя класса текущей модели
        /// </summary>
        public string ClassName
        {
            get { return this.GetType().ToString().Split('.').Last(); }
        } //---свойство ClassName
        #endregion

        #region Команды
        private RelayCommand command_select_model;
        /// <summary>
        /// Команда, вызывающее событие выбора данной модели
        /// </summary>
        public virtual RelayCommand Command_select_model
        {
            get
            {
                return this.command_select_model ?? (this.command_select_model = new RelayCommand(obj =>
                {
                    this.Event_select_model?.Invoke(this);
                }));
            }
        } //---команда Command_select_model
        #endregion
    }
}
