using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mr.MVVM
{
    /// <summary>
    /// Наследуется от <see cref="ObservableCollection{T}"/>, расширяя функционал
    /// </summary>
    /// <typeparam name="T">Принимает любой <see cref="object"/></typeparam>
    public partial class ListExt<T> : ObservableCollection<T>
    {
        #region Констукторы
        /// <summary>
        /// Конструктор
        /// </summary>
        public ListExt() : base() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="collection">Принимает коллекцию, из которой создает массив элементов</param>
        public ListExt(IEnumerable<T> collection) : base(collection) { }
        #endregion

        #region Методы
        /// <summary>
        /// Добавляет коллекцию в конец массива
        /// </summary>
        /// <param name="collection">Принимает коллекцию для добавления</param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T model in collection)
            {
                this.Add(model);
            }
        } //---метод AddRange

        /// <summary>
        /// Добавляет коллекцию в указанный индекс массива
        /// </summary>
        /// <param name="index">Принимает индекс</param>
        /// <param name="collection">Принимает коллекцию для добавления</param>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (index < 0 && index >= this.Count()) return;
            foreach (T model in Inverse(collection))
            {
                this.Insert(index, model);
            }
        } //---метод InsertRange

        /// <summary>
        /// Удаляет из данного массива элементы коллекции; сравнивает объекты массива и коллекции как критерий
        /// </summary>
        /// <param name="collection">Принимает коллекцию элементов для удаления</param>
        public void RemoveRange(IEnumerable<T> collection)
        {
            foreach (T remove in collection)
            {
                foreach (T model in this)
                {
                    if (model.Equals(remove))
                    {
                        this.Remove(model);
                        break;
                    }
                }
            }
        } //---метод RemoveRange

        /// <summary>
        /// Инвертирует полученную коллекцию
        /// </summary>
        /// <param name="collection">Принимает коллекцию</param>
        /// <returns>Возвращает инвертированную коллекцию</returns>
        public static T[] Inverse(IEnumerable<T> collection)
        {
            T[] tmp_send = new T[collection.Count()];

            for (int i = collection.Count() - 1, j = 0; i >= 0; i--, j++)
            {
                tmp_send[j] = collection.ElementAt(i);
            }

            return tmp_send;
        } //---метод Inverse

        /// <summary>
        /// Инвертирует текущую коллекцию
        /// </summary>
        /// <returns>Возвращает инвертированную текущую коллекцию</returns>
        public ListExt<T> Inverse()
        {
            return new ListExt<T>(Inverse(this));
        }//---метод Inverse
        #endregion
    } //---класс ListExt<T>
}
