namespace Mementorize.Models
{
    public class Memento<T> where T : new()
    {
        private T State { get; set; }

        public void SetState(T state) => State = state;

        public T GetState() => State;
    }
}