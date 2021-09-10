using System;
using System.Globalization;
using MementoSharp;
using Newtonsoft.Json;

namespace ConsoleApp.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new ExampleClass();

            x.RunIt();
            
            Console.ReadKey();
            
            Console.WriteLine(MementoExtension.SavedObjectsCount());  //should be 0 because gc should collected x . that means extension work well on monitoring object life and gc.
        }
    }
    
    public class ExampleClass
    {
        public void RunIt()
        {
            var instance = new WithParameterlessConstructorClass(1, "One", new[] {1, 2, 3});
            var instanceTwo = new WithoutParameterlessConstructorClass(500, new[] {500, 501, 502});
            instanceTwo.SetName("FiveHundred");


            instance.SaveState(new JsonSerializerSettings()
            {
                Culture = CultureInfo.InvariantCulture
            });
            instanceTwo.SaveState();

            instance.Id = 2;
            instance.Name = "Two";
            instance.Nestes.Numbers = new[] {4, 5, 6};
            instance.SaveState();

            instanceTwo.Id = 600;
            instanceTwo.SetName("SixHundred");
            instanceTwo.Nestes.Numbers = new[] {600, 601, 602};
            instanceTwo.SaveState();


            instance.Id = 3;
            instance.Name = "Three";
            instance.Nestes.Numbers = new[] {7, 8, 9};
            instance.SaveState();

            instanceTwo.Id = 700;
            instanceTwo.SetName("SevenHundred");
            instanceTwo.Nestes.Numbers = new[] {700, 701, 702};
            instanceTwo.SaveState();


            var undo1 = instance.RestoreState(0,new JsonSerializerSettings());
            var undo2 = instance.RestoreState(1);
            var undo3 = instance.RestoreState(2);


            Console.WriteLine(instance.StatesCount());
            Console.WriteLine(JsonConvert.SerializeObject(undo1));
            Console.WriteLine(JsonConvert.SerializeObject(undo2));
            Console.WriteLine(JsonConvert.SerializeObject(undo3));

            var undo500 = instanceTwo.RestoreState<TestReadmodel>(0);
            var undo600 = instanceTwo.RestoreState<TestReadmodel>(1);
            var undo700 = instanceTwo.RestoreState<TestReadmodel>(2);

            Console.WriteLine(instanceTwo.StatesCount());
            Console.WriteLine(JsonConvert.SerializeObject(undo500));
            Console.WriteLine(JsonConvert.SerializeObject(undo600));
            Console.WriteLine(JsonConvert.SerializeObject(undo700));
        }
    }


    class WithParameterlessConstructorClass
    {
        public WithParameterlessConstructorClass(int id, string name, int[] numbers)
        {
            Id = id;
            Name = name;
            Nestes = new Nested(numbers);
        }

        public WithParameterlessConstructorClass()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nested Nestes { get; set; }
    }


    //with encapsulated member
    class WithoutParameterlessConstructorClass
    {
        public WithoutParameterlessConstructorClass(int id, int[] numbers)
        {
            Id = id;
            Nestes = new Nested(numbers);
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; private set; }
        public Nested Nestes { get; set; }
    }

    class TestReadmodel
    {
        public string Name { get; set; }
        public Nested Nestes { get; set; }
    }

    class Nested
    {
        public Nested(int[] numbers)
        {
            Numbers = numbers;
        }

        public int[] Numbers { get; set; }
    }
}