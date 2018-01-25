using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicSample3_CA
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic expand = new ExpandoObject();

            expand.Name = "Rick";
            expand.HelloWorld = (Func<string, string>)((string name) =>
            {
                return "Hello " + name;
            });

            Console.WriteLine(expand.Name);
            Console.WriteLine(expand.HelloWorld2("Dufus"));
            Console.ReadKey();
        }
    }
}
