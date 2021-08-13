using System;
using AutoFixture;
using Xunit;

namespace Memento.Extension.Test
{
    public class ObjectExtensionTest
    {
        [Fact]
        public void Should_Take_and_Restore_SnapShot_Correctly()
        {
            const string name = "name-test1";
            const int number = 5;

            var fakeClass = new FakeClass() {Name = name, Number = number};

            fakeClass.CreateSnapshot();

            fakeClass.Name = Guid.NewGuid().ToString();
            fakeClass.Number = new Random().Next(1, 10);

            fakeClass = fakeClass.ReturnSnapshot(0);

            Assert.Equal("name-test1", fakeClass.Name);
            Assert.Equal(5, fakeClass.Number);
        }

        [Fact]
        public void Should_Clear_Memory_When_GC_Collected_Target_Object()
        {
            const string name = "name-test1";
            const int number = 5;

            var fakeClass = new FakeClass() {Name = name, Number = number};

            fakeClass.CreateSnapshot();

            fakeClass = new Fixture().Build<FakeClass>().Create();  // Gc Collect the old object

            fakeClass = fakeClass.ReturnSnapshot(0);

            Assert.Null(fakeClass);
        }
    }

    public class FakeClass
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
}