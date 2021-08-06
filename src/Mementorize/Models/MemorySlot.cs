using System;

namespace Mementorize.Models
{
    public class MemorySlot
    {
        public WeakReference CurrentObject { get; set; }
        
        public CareTaker<string> CareTaker { get; set; }
    }
}