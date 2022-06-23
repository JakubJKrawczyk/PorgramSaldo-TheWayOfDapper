// See https://aka.ms/new-console-template for more information


using System.Dynamic;

dynamic expObj = new ExpandoObject();
expObj.Test = string.Empty as string;
Console.WriteLine(expObj.Test);