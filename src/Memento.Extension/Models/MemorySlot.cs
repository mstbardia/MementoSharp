using System;

namespace Memento.Extension.Models
{
    /// <summary>
    /// the class that keeps low reference on your object and its caretaker
    /// to use it save or restore state and be aware of object is
    /// exist or collected.
    /// </summary>
    internal sealed class MemorySlot
    {

        public MemorySlot()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; private set; }
        
        public WeakReference CurrentObject { get; set; }
        
        public CareTaker<string> CareTaker { get; set; }
    }
}