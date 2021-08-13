using System;
using System.Text.Json;
using Memento.Extension;
using Memento.Extension.Abstractions;
using Memento.Extension.Models;

namespace ConsoleApp.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectExtensionTest();
            ImmutableTypeTest();
            MutableTypeTest();
        }

        static void ObjectExtensionTest()
        {
            var instance = new TestOne(1, "One", new[] {1, 2, 3});
            instance.CreateSnapshot();

            instance.Id = 2;
            instance.Name = "Two";
            instance.Nestes.Numbers = new[] {4, 5, 6};
            instance.CreateSnapshot();

            instance.Id = 3;
            instance.Name = "Three";
            instance.Nestes.Numbers = new[] {7, 8, 9};
            instance.CreateSnapshot();


            var undo1 = instance.ReturnSnapshot(0);
            var undo2 = instance.ReturnSnapshot(1);
            var undo3 = instance.ReturnSnapshot(2);

            Console.WriteLine(instance.SnapshotsCount());
            Console.WriteLine(JsonSerializer.Serialize(undo1));
            Console.WriteLine(JsonSerializer.Serialize(undo2));
            Console.WriteLine(JsonSerializer.Serialize(undo3));
        }

        static void ImmutableTypeTest()
        {
            var test = "hi";

            var org = new Originator<string>();
            var care = new CareTaker<string>();
            org.SetState(test);
            care.SaveMemento(org);

            test = "bye";
            org.SetState(test);
            care.SaveMemento(org);

            test = "fuck";
            org.SetState(test);
            care.SaveMemento(org);


            care.RestoreMemento(org, 0);
            test = org.GetState();
            Console.WriteLine(JsonSerializer.Serialize(test)); // this should be = hi
        }

        static void MutableTypeTest()
        {
            var car = new Car("benz", 300);

            var org = new Originator<Car>();
            var care = new CareTaker<Car>();
            org.SetState(car.Clone());
            care.SaveMemento(org);

            car.Model = "bmw";
            car.MaxSpeed = 150;
            org.SetState(car.Clone());
            care.SaveMemento(org);

            car.Model = "nissan";
            car.MaxSpeed = 50;
            org.SetState(car.Clone());
            care.SaveMemento(org);

            care.RestoreMemento(org, 0);
            car = org.GetState();
            Console.WriteLine(JsonSerializer.Serialize(car)); // this should be  { benz , 300 }

            care.RestoreMemento(org, 1);
            car = org.GetState();
            Console.WriteLine(JsonSerializer.Serialize(car)); // this should be  { bmw , 150 }
        }
    }


    public class Car
    {
        public string Model { get; set; }
        public int MaxSpeed { get; set; }
        public Car(string model, int maxSpeed)
        {
            Model = model;
            MaxSpeed = maxSpeed;
        }
        public Car Clone()
        {
            return new Car(this.Model, this.MaxSpeed);
        }
    }
    class TestOne
    {
        public TestOne(int id, string name, int[] numbers)
        {
            Id = id;
            Name = name;
            Nestes = new Nested(numbers);
        }
        public TestOne()
        {
            //parameter constructor require for extension
        }
        public int Id { get; set; }
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