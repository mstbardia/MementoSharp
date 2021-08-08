namespace Mementorize.Models
{
    /// <summary>
    /// the class that hold state encapsulated in this pattern 
    /// </summary>
    /// <typeparam name="T">type of your keeping state</typeparam>
    public class Memento<T>
    {
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