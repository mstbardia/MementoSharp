namespace Mementorize.Models
{
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