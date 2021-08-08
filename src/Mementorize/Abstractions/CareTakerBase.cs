using System.Collections.Generic;
using Mementorize.Models;

namespace Mementorize.Abstractions
{
    /// <summary>
    ///  Caretaker is responsible for save and restore originator state
    ///  by memento saved history
    /// </summary>
    /// <typeparam name="T">type of your keeping state</typeparam>
    public abstract class CareTakerBase<T>
    {
        protected List<Memento<T>> Mementoes { get; private set; }

        protected CareTakerBase()
        {
            Mementoes = new List<Memento<T>>();
        }
        public virtual void SaveMemento(Originator<T> originator)
        {
            Mementoes.Add(originator.CreateMementoFromState());
        }
        public virtual void RestoreMemento(Originator<T> originator, int mementoIndex)
        {
            originator.RestoreStateFromMemento(Mementoes[mementoIndex]);
        }
        public virtual int MementosCount()
        {
            return Mementoes.Count;
        }
    }
}