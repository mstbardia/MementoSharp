using System;
using System.Text.Json;

namespace Memento.Extension.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new TestOne()
            {
                Id = 1,
                Name = "one",
                Nestes = new Nested()
                {
                    Numbers = new[] {1, 2, 3}
                }
            };
            
            instance.CreateSnapshot();
            
            instance.Id = 2;
            instance.Name = "Two";
            instance.Nestes.Numbers = new[] {4, 5, 6};

            instance.CreateSnapshot();

            instance.Id = 3;
            instance.Name = "Three";
            instance.Nestes.Numbers = new[] {7, 8, 9};

            instance.CreateSnapshot();

            
            var undo1 = (TestOne) instance.ReturnSnapshot(0);
            var undo2 = (TestOne) instance.ReturnSnapshot(1);
            var undo3 = (TestOne) instance.ReturnSnapshot(2);
            
            Console.WriteLine(instance.SnapshotsCount());
            Console.WriteLine(JsonSerializer.Serialize(undo1));
            Console.WriteLine(JsonSerializer.Serialize(undo2));
            Console.WriteLine(JsonSerializer.Serialize(undo3));
             
        }
    }

    class TestOne 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nested Nestes { get; set; }
    }

    class Nested
    {
        public int[] Numbers { get; set; }
    }
}