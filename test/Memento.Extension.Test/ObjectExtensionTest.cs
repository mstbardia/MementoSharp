using System;
using AutoFixture;
using Xunit;

namespace Memento.Extension.Test
{
    public class ObjectExtensionTest
    {
        [Fact]
        public void Should_Take_and_Restore_state_Correctly()
        {
            const string name = "name-test1";
            const int number = 5;

            var fakeClass = new FakeClass() {Name = name, Number = number};

            fakeClass.SaveState();

            fakeClass.Name = Guid.NewGuid().ToString();
            fakeClass.Number = new Random().Next(1, 10);

            fakeClass = fakeClass.RestoreState(0);

            Assert.Equal("name-test1", fakeClass.Name);
            Assert.Equal(5, fakeClass.Number);
        }

        [Fact]
        public void Should_Return_Null_When_Target_Instance_Changed()
        {
            const string name = "name-test1";
            const int number = 5;

            var fakeClass = new FakeClass() {Name = name, Number = number};

            fakeClass.SaveState();

            fakeClass = new Fixture().Build<FakeClass>().Create();  // Gc Collect the old object

            fakeClass = fakeClass.RestoreState(0);

            Assert.Null(fakeClass);
        }
    }

    public class FakeClass
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
}