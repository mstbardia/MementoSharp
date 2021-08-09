using System;

namespace Memento.Extension.Models
{
    /// <summary>
    /// the class that keeps low reference on your object and its caretaker
    /// to use it save or restore state and be aware of object is
    /// exist or collected.
    /// </summary>
    public class MemorySlot
    {
        public WeakReference CurrentObject { get; set; }
        
        public CareTaker<string> CareTaker { get; set; }
    }
}