# Memento Extension

Memento is design pattern that capture object internal state , so the object
can be restored to this state later.
this project provide a extension on object type that create multiple snapshot of your
object and restore state by snapshot id as long as the object exist.



**Example :**

        `var instance = new TestOne(1, "One", new[] {1, 2, 3});
             
             instance.CreateSnapshot();

             instance.Id = 2;
             instance.Name = "Two";
             instance.Nestes.Numbers = new[] {4, 5, 6};

             var undo1 = instance.ReturnSnapshot(0);  // result => {"Id":1,"Name":"One","Nestes":{"Numbers":[1,2,3]}}`


**Note :**

you can use memento pattern by your own implementation , all abstraction and models exist in generic type and
methods are virtual.

