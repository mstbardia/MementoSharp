using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Timers;
using Memento.Extension.Models;

namespace Memento.Extension
{
    public static class MementoExtension
    {
        private static readonly List<MemorySlot> MemorySlots = new();
        private static Timer GcTimer { get; set; }
        
        /// <summary>
        ///  Serialize object to json and put it in memento
        ///  design pattern
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="jsonSerializerOptions">json option - this method process your object state as json</param>
        public static void SaveState<T>(this T obj,
            JsonSerializerOptions jsonSerializerOptions = null) where T : class
        {
            RefreshMemorySlots();
            var currentStateJson = JsonSerializer.Serialize(obj, jsonSerializerOptions);
            var objectOriginator = new Originator<string>();
            objectOriginator.SetState(currentStateJson);
            var careTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (careTaker == null)
            {
                careTaker = new CareTaker<string>();
                MemorySlots.Add(new MemorySlot() {CareTaker = careTaker, CurrentObject = new WeakReference(obj)});
            }
            careTaker.SaveMemento(objectOriginator);
            if (GcTimer == null)
                InitiateGcTimer();
        }


        /// <summary>
        ///  deserialize stored state of object and return it in type
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="stateIndex">backward step that old object state</param>
        /// <param name="jsonSerializerOptions">json option - this method process your object state as json</param>
        /// <returns></returns>
        public static T RestoreState<T>(this T obj, int stateIndex,
            JsonSerializerOptions jsonSerializerOptions = null) where T : class, new()
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker is null)
                return null;
            var objectOriginator = new Originator<string>();
            existCareTaker.RestoreMemento(objectOriginator, stateIndex);
            return JsonSerializer.Deserialize<T>(objectOriginator.GetState(), jsonSerializerOptions);
        }
        
        public static TOut RestoreState<TOut>(this object obj, int stateIndex,
            JsonSerializerOptions jsonSerializerOptions = null) where TOut : class
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker is null)
                return null;
            var objectOriginator = new Originator<string>();
            existCareTaker.RestoreMemento(objectOriginator, stateIndex);
            return JsonSerializer.Deserialize<TOut>(objectOriginator.GetState(), jsonSerializerOptions);
        }


        /// <summary>
        ///  return count of existed state of this object in history
        /// </summary>
        /// <param name="obj">target object</param>
        /// <returns></returns>
        public static int SavedStatesCount(this object obj)
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            return existCareTaker?.MementosCount() ?? 0;
        }


        //clear history from collected object  
        private static void RefreshMemorySlots()
        {
            GC.Collect();
            MemorySlots.RemoveAll(c => c.CurrentObject.IsAlive == false);
        }
        
        //initiate timer to call GC periodically to clear memory list if target object collected
        private static void InitiateGcTimer()
        {
            if (GcTimer == null)
            {
                GcTimer = new Timer(1000);
                GcTimer.Elapsed += GcTimerOnElapsed;
                GcTimer.AutoReset = false;
                GcTimer.Enabled = true;
            }
        }
        
        //gc timer do
        private static void GcTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            RefreshMemorySlots();
            Console.WriteLine(MemorySlots.Count);
            if (MemorySlots.Count != 0)
                GcTimer.Start();
            GcTimer.Dispose();
        }
    }
}