using Mementorize.Models;

namespace Mementorize.Abstractions
{
    public abstract class OrginatorBase<T> where T : new()
    {
        protected T State { get; set; }

        public virtual void SetState(T state)
        {
            State = state;
        }

        public virtual T GetState()
        {
            return State;
        }
        
        public virtual Memento<T> CreateMemento()
        {
            var memento = new Memento<T>();
            memento.SetState(State);
            return memento;   
        }
        
    }
}