using System.Collections.Generic;
using Mementorize.Models;

namespace Mementorize.Abstractions
{
    public abstract class CareTakerBase<T>
    {
        protected readonly List<Memento<T>> Mementoes;

        protected CareTakerBase()
        {
            Mementoes = new List<Memento<T>>();
        }

        public virtual void SaveMemento(Orginator<T> orginator)
        {
            Mementoes.Add(orginator.CreateMementoFromState());
        }

        public virtual void RestoreMemento(Orginator<T> orginator,int checkpointIndex)
        {
            orginator.RestoreState(Mementoes[checkpointIndex]);
        }

        public virtual int MementosCount()
        {
            return Mementoes.Count;
        }
    }
}