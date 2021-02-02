# Finale.Json

Finale.Json is a lightweight .NET library for managing JSON data.

## How to use
A `JsonObject` or `JsonArray` can be created empty, by passing a JSON-formated string, or by passing members to the constructor directly. `JsonObject` behaves like a `Dictionary<>`, while `JsonArray` behaves like a `List<>`.

### JsonObject
```c#
//create an empty object, then add members to it
JsonObject jo1 = new JsonObject();
jo1["name"] = "apple";

//create from a JSON-formated string
JsonObject jo2 = new JsonObject("{\"name\": \"apple\"}");

//create from an annoymous type
JsonObject jo3 = new JsonObject(new{
    name = "apple"
});

Console.WriteLine(jo1["name"]);              //apple
Console.WriteLine(jo1.ToString());           //{"name":"apple"}
Console.WriteLine(jo1.Count);                //1
Console.WriteLine(jo1.ContainsKey("name"))   //True
```

### JsonArray
```c#
//create an empty array, then add members to it
JsonArray ja1 = new JsonArray();
ja1.Add(123);

//create from JSON-formated string
JsonArray ja2 = new JsonArray("[123]");

//create from array variable
JsonArray ja3 = new JsonArray(new int[] { 123 });

Console.WriteLine(ja1[0]);                  //123
Console.WriteLine(ja1.ToString());          //[123]
Console.WriteLine(ja1.Count);               //1
```
