using Mementorize.Models;

namespace Mementorize.Abstractions
{
    public abstract class OrginatorBase<T> where T : new()
    {
        protected T State { get; private set; }

        public virtual void SetState(T state)
        {
            State = state;
        }

        public virtual T GetState()
        {
            return State;
        }
        
        public virtual Memento<T> CreateMementoFromState()
        {
            var memento = new Memento<T>();
            memento.SetState(State);
            return memento;   
        }
        
        public virtual void RestoreState(Memento<T> memento)
        {
            var mementoState = memento.GetState();
            SetState(mementoState);
        }
    }
}