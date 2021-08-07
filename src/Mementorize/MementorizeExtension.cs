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

        public static void CreateSnapshot(this object obj,
            JsonSerializerOptions jsonSerializerOptions = null)
        {
            RefreshMemerySlots();
            var currentStateJson = JsonSerializer.Serialize(obj, jsonSerializerOptions);
            var objectOrginator = new Orginator<string>();
            objectOrginator.SetState(currentStateJson);
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
            {
                existCareTaker = new CareTaker<string>();
                MemorySlots.Add(new MemorySlot() {CareTaker = existCareTaker, CurrentObject = new WeakReference(obj)});
            }
            existCareTaker.SaveMemento(objectOrginator);
        }


        public static object ReturnSnapshot(this object obj, int snapshotIndex,
            JsonSerializerOptions jsonSerializerOptions = null)
        {
            RefreshMemerySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
                return null;
            var objectOrginator = new Orginator<string>();
            existCareTaker.RestoreMemento(objectOrginator, snapshotIndex);
            return JsonSerializer.Deserialize(objectOrginator.GetState(), obj.GetType(), jsonSerializerOptions);
        }


        public static int SnapshotsCount(this object obj)
        {
            RefreshMemerySlots();
            var existCareTaker = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTaker == null)
                return 0;
            return existCareTaker.MementosCount();
        }


        private static void RefreshMemerySlots()
        {
            GC.Collect();
            MemorySlots.RemoveAll(c => c.CurrentObject.IsAlive == false);
        }
    }
}