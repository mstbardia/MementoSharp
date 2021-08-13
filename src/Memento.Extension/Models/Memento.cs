namespace Memento.Extension.Models
{
    /// <summary>
    /// the class that hold state encapsulated in this pattern 
    /// </summary>
    /// <typeparam name="T">type of your keeping state</typeparam>
    public class Memento<T>
    {
        /// <summary>
        /// Note That !!!
        /// if you want use it for mutable reference type (class , ...)
        /// you should pass a clone of your target object to state
        /// </summary>
        private T State { get; set; }
        public void SetState(T state)
        {
            State = state;
        }
        public T GetState()
        {
            return State;
        }
    }
}