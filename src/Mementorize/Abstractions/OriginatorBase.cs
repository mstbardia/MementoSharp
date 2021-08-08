using Mementorize.Models;

namespace Mementorize.Abstractions
{
    /// <summary>
    ///  the main class that should create memento from its own state
    ///  in memento design pattern this class is your main object
    ///  but here i designed it as generic to use it more than one time in different type , you can override it
    ///  in way you like. 
    /// </summary>
    /// <typeparam name="T">type of your keeping state</typeparam>
    public abstract class OriginatorBase<T>
    {
        protected T State { get; private set; }
        
        
        /// <summary>
        /// set state from <typeparamref name="T"/> type state
        /// </summary>
        /// <typeparam name="T">type of your keeping state</typeparam>
        public virtual void SetState(T state)
        {
            State = state;
        }

        /// <summary>
        /// return state of <typeparamref name="T"/> type
        /// </summary>
        /// <typeparam name="T">type of your keeping state</typeparam>
        public virtual T GetState()
        {
            return State;
        }

        /// <summary>
        /// create memento of <typeparamref name="T"/> and set
        /// state of it from state
        /// </summary>
        /// <typeparam name="T">type of your keeping state</typeparam>
        public virtual Memento<T> CreateMementoFromState()
        {
            var memento = new Memento<T>();
            memento.SetState(State);
            return memento;   
        }
        
        /// <summary>
        /// return memento of <typeparamref name="T"/> and set
        /// state from it
        /// </summary>
        /// <typeparam name="T">type of your keeping state</typeparam>
        public virtual void RestoreStateFromMemento(Memento<T> memento)
        {
            var mementoState = memento.GetState();
            SetState(mementoState);
        }
    }
}