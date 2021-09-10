using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using MementoSharp.Internal;
using Newtonsoft.Json;

namespace MementoSharp
{
    public static class MementoExtension
    {
        private static readonly List<MemorySlot> MemorySlotList = new();
        private static Timer GcTimer { get; set; }

        /// <summary>
        ///  Serialize object to json and put it in memento
        ///  design pattern
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="jsonSerializerSettings">json option - this method process your object state as json</param>
        public static void SaveState<T>(this T obj,
            JsonSerializerSettings jsonSerializerSettings = null)  
        {
            RefreshMemorySlots();
            var currentStateJson = JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            var objectOriginator = new Originator<string>();
            objectOriginator.SetState(currentStateJson);
            var careTaker = MemorySlotList
                .SingleOrDefault(c => c.CurrentObject.Target != null && c.CurrentObject.Target.Equals(obj))?.CareTaker;
            if (careTaker == null)
            {
                careTaker = new CareTaker<string>();
                MemorySlotList.Add(new MemorySlot() {CareTaker = careTaker, CurrentObject = new WeakReference(obj)});
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
        /// <param name="jsonSerializerSettings">json option - this method process your object state as json</param>
        /// <returns></returns>
        public static T RestoreState<T>(this T obj, int stateIndex,
            JsonSerializerSettings jsonSerializerSettings = null) 
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlotList
                .SingleOrDefault(c => c.CurrentObject.Target != null && c.CurrentObject.Target.Equals(obj))?.CareTaker;
            if (existCareTaker is null)
                return default;
            var objectOriginator = new Originator<string>();
            existCareTaker.RestoreMemento(objectOriginator, stateIndex);
            return JsonConvert.DeserializeObject<T>(objectOriginator.GetState(), jsonSerializerSettings);
        }

        /// <summary>
        ///  deserialize stored state of object and return it in selected type
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="stateIndex">backward step that old object state</param>
        /// <param name="jsonSerializerSettings">json option - this method process your object state as json</param>
        /// <returns></returns>
        public static TOut RestoreState<TOut>(this object obj, int stateIndex,
            JsonSerializerSettings jsonSerializerSettings = null)  
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlotList
                .SingleOrDefault(c => c.CurrentObject.Target != null && c.CurrentObject.Target.Equals(obj))?.CareTaker;
            if (existCareTaker is null)
                return default;
            var objectOriginator = new Originator<string>();
            existCareTaker.RestoreMemento(objectOriginator, stateIndex);
            return JsonConvert.DeserializeObject<TOut>(objectOriginator.GetState(), jsonSerializerSettings);
        }

        /// <summary>
        ///  return count of existed state of this object in history
        /// </summary>
        /// <param name="obj">target object</param>
        /// <returns></returns>
        public static int StatesCount(this object obj)
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlotList.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            return existCareTaker?.MementosCount() ?? 0;
        }

        /// <summary>
        ///  return count of entire object exist in memento
        /// </summary>
        /// <returns></returns>
        public static int SavedObjectsCount()
        {
            RefreshMemorySlots();
            return MemorySlotList.Count;
        }


        //clear history from collected object  
        private static void RefreshMemorySlots()
        {
            GC.Collect();
            MemorySlotList.RemoveAll(c => c.CurrentObject.IsAlive == false);
        }

        //initiate timer to clear memory list if target object collected
        private static void InitiateGcTimer()
        {
            if (GcTimer != null) return;
            GcTimer = new Timer(300000); // run per 5 min
            GcTimer.Elapsed += GcTimerOnElapsed;
            GcTimer.AutoReset = false;
            GcTimer.Enabled = true;
        }

        //gc timer do
        private static void GcTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            RefreshMemorySlots();
            if (MemorySlotList.Count != 0)
                GcTimer.Start();
            GcTimer.Dispose();
        }
    }
}