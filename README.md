# MementoSharp

MementoSharp is a extension of memento design pattern on object type , to save state of object
and restore it on runtime by state index.
the extension also monitor the life of object to release its memory of object when gc collect the target
object.


**Simple Example :**
```c#
        var instance = new TestOne(1, "One", new[] {1, 2, 3});
             
            instance.SaveState();

            instance.Id = 2;
            instance.Name = "Two";
            instance.Nestes.Numbers = new[] {4, 5, 6};


            instance.SaveState();

            instance.Id = 3;
            instance.Name = "Three";
            instance.Nestes.Numbers = new[] {7, 8, 9};


            instance.SaveState();

            var undo1 = instance.RestoreState(0);  // result => {"Id":1,"Name":"One","Nestes":{"Numbers":[1,2,3]}}
            var undo2 = instance.RestoreState(1);  // result => {"Id":2,"Name":"Two","Nestes":{"Numbers":[4,5,6]}}
```

MementoSharp use json serialization to keep state so you can pass JsonSerializerSettings
to save or restore method if you use complex object to serialize.

**Example :**
 ```c#           
            var instance = new TestOne(1, "One", new[] {1, 2, 3});

            instance.SaveState(new JsonSerializerSettings()
            {
                Culture = CultureInfo.InvariantCulture
            });

            instance.Id = 2;
            instance.Name = "Two";
            instance.Nestes.Numbers = new[] {4, 5, 6};

            var undo1 = instance.RestoreState(0, new JsonSerializerSettings(){
                Culture = CultureInfo.InvariantCulture
            });
```
also two method include to show how many states of target object exist and also how many 
object are exist on extension.
 ```c#      
            MementoExtension.SavedObjectsCount();  // give the entire count of objects exist on memento
            instance.StatesCount(); // give states count of target instance
```
