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
        
        public static bool CreateSnapshot(this object obj)
        {
            RefreshMemerySlots();

            var currentStateJson = JsonSerializer.Serialize(obj);
            var objectOrginator = new Orginator<string>();
            objectOrginator.SetState(currentStateJson);
            
            var existCareTake = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;

            if (existCareTake != null)
            {
                existCareTake.SaveMemento(objectOrginator);
            }
            else
            {
                var careTaker = new CareTaker<string>();
                careTaker.SaveMemento(objectOrginator);
                var memorySlot = new MemorySlot() {CareTaker = careTaker, CurrentObject = new WeakReference(obj)};
                MemorySlots.Add(memorySlot);
            }

            return true;
        }


        public static object ReturnSnapshot(this object obj , int snapshotIndex)
        {
            RefreshMemerySlots();
            var existCareTake = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            if (existCareTake != null)
            {
                var objectOrginator = new Orginator<string>();
                existCareTake.RestoreMemento(objectOrginator,snapshotIndex);
                return JsonSerializer.Deserialize(objectOrginator.GetState(), obj.GetType());
            }

            return null;
        }


        public static int SnapShotCount(this object obj)
        {
            var existCareTake = MemorySlots.SingleOrDefault(c => c.CurrentObject.Target == obj)?.CareTaker;
            return existCareTake?.MementosCount() ?? 0;
        }
        
        
        private static void RefreshMemerySlots()
        {
            GC.Collect();
            MemorySlots.RemoveAll(c => c.CurrentObject.IsAlive == false);
        }
    }
}