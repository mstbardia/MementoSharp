using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Mementorize.Models;

namespace Mementorize
{
    public static class MementorizeExtension
    {
        private static readonly List<MemorySlot> MemorySlots = new();

        /// <summary>
        ///  Serialize object to json and put it in memento
        ///  design pattern
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="jsonSerializerOptions">json</param>
        public static void CreateSnapshot(this object obj,
            JsonSerializerOptions jsonSerializerOptions = null)
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


        
        public static object ReturnSnapshot(this object obj, int snapshotIndex,
            JsonSerializerOptions jsonSerializerOptions = null)
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
                return null;
            var objectOriginator = new Originator<string>();
            existCareTaker.RestoreMemento(objectOriginator, snapshotIndex);
            return JsonSerializer.Deserialize(objectOriginator.GetState(), obj.GetType(), jsonSerializerOptions);
        }


        public static int SnapshotsCount(this object obj)
        {
            RefreshMemorySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
                return 0;
            return existCareTaker.MementosCount();
        }


        private static void RefreshMemorySlots()
        {
            GC.Collect();
            MemorySlots.RemoveAll(c => c.CurrentObject.IsAlive == false);
        }
    }
}