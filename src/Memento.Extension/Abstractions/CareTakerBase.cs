using System.Collections.Generic;
using Memento.Extension.Models;

namespace Memento.Extension.Abstractions
{
    /// <summary>
    ///  Caretaker is responsible for save and restore originator state
    ///  by memento saved history
    /// </summary>
    /// <typeparam name="T">type of your keeping state</typeparam>
    internal abstract class CareTakerBase<T>
    {
        protected List<Memento<T>> Mementoes { get; private set; }

        protected CareTakerBase()
        {
            Mementoes = new List<Memento<T>>();
        }
        
        /// <summary>
        /// get memento from originator and save it in memento history list
        /// </summary>
        /// <param name="originator">originator of your type</param>
        public virtual void SaveMemento(Originator<T> originator)
        {
            Mementoes.Add(originator.CreateMementoFromState());
        }
        
        /// <summary>
        ///  extract memento from memento history list by id
        /// </summary>
        /// <param name="originator">originator of your type</param>
        /// <param name="mementoIndex">id of memento in history list</param>
        public virtual void RestoreMemento(Originator<T> originator, int mementoIndex)
        {
            originator.RestoreStateFromMemento(Mementoes[mementoIndex]);
        }
        
        /// <summary>
        /// return memento count in history list
        /// </summary>
        /// <returns></returns>
        public virtual int MementosCount()
        {
            return Mementoes.Count;
        }
    }
}