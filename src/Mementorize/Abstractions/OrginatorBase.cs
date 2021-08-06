using Mementorize.Models;

namespace Mementorize.Abstractions
{
    public abstract class OrginatorBase<T>
    {
        protected T State { get; set; }
        
        public virtual void SetState(T state) => State = state;
        
        public virtual T GetState() => State;
        
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