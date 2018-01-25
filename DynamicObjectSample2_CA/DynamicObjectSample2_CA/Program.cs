using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectSample2_CA
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic expObj = new ExpandoObject();
            ((INotifyPropertyChanged)expObj).PropertyChanged +=
                new PropertyChangedEventHandler(HandlePropertyChanges);
            expObj.Ad = "Mustafa";
            expObj.Soyad = "Sacli";
            expObj.Id = 12;

            expObj.GetTableName = new Func<string>(() =>
            {
                return "Person";
            });
            expObj.GetIdCol = new Func<string>(() => { return "Id"; });


            //Console.WriteLine("Type: {0}", expObj.GetType().Name);
            IDictionary<string, object> idict = (IDictionary<string, object>)expObj;
            foreach (var item in idict)
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            }

            Console.WriteLine("Table: {0}", expObj.GetTableName());
            Console.WriteLine("Id Column: {0}", expObj.GetIdCol());
            Console.ReadLine();
        }

        private static void HandlePropertyChanges(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("Changed Property: {0}", e.PropertyName);
        }
    }
}
