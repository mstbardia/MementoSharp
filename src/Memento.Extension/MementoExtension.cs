using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Memento.Extension.Models;

namespace Memento.Extension
{
    public static class MementoExtension
    {
        private static readonly List<MemorySlot> MemorySlots = new();

        /// <summary>
        ///  Serialize object to json and put it in memento
        ///  design pattern
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="jsonSerializerOptions">json option - this method process your object state as json</param>
        public static void CreateSnapshot<T>(this T obj,
            JsonSerializerOptions jsonSerializerOptions = null) where T : class , new() 
        {
            RefreshMemorySlots();
            var currentStateJson = JsonSerializer.Serialize(obj, jsonSerializerOptions);
            var objectOriginator = new Originator<string>();
            objectOriginator.SetState(currentStateJson);
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
            {
                existCareTaker = new CareTaker<string>();
                MemorySlots.Add(new MemorySlot() {CareTaker = existCareTaker, CurrentObject = new WeakReference(obj)});
            }
            existCareTaker.SaveMemento(objectOriginator);
        }


        /// <summary>
        ///  deserialize stored snapshot of object and return it in type
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="snapshotIndex">backward step that old object state</param>
        /// <param name="jsonSerializerOptions">json option - this method process your object state as json</param>
        /// <returns></returns>
        public static T ReturnSnapshot<T>(this T obj, int snapshotIndex,
            JsonSerializerOptions jsonSerializerOptions = null) where T : class , new()
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
                return null;
            var objectOriginator = new Originator<string>();
            existCareTaker.RestoreMemento(objectOriginator, snapshotIndex);
            return JsonSerializer.Deserialize<T>(objectOriginator.GetState(),jsonSerializerOptions);
        }

        /// <summary>
        ///  return count of existed state of this object in history
        /// </summary>
        /// <param name="obj">target object</param>
        /// <returns></returns>
        public static int SnapshotsCount(this object obj)
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
                return 0;
            return existCareTaker.MementosCount();
        }


        //clear history from collected object  
        private static void RefreshMemorySlots()
        {
            GC.Collect();
            MemorySlots.RemoveAll(c => c.CurrentObject.IsAlive == false);
        }
    }
}