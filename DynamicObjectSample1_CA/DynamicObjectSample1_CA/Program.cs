using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectSample1_CA
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic MyDynamic = new ExpandoObject();
            MyDynamic.A = "A";
            MyDynamic.B = "B";
            MyDynamic.C = "C";
            MyDynamic.SomeProperty = "My Name is Muty";
            MyDynamic.number = 10;
            MyDynamic.Increment = (Action)(() => { MyDynamic.number++; });

            Console.WriteLine("{0}", MyDynamic.A);
            Console.WriteLine("{0}", MyDynamic.B);
            Console.WriteLine("{0}", MyDynamic.C);
            Console.WriteLine("{0}", MyDynamic.SomeProperty);
            Console.WriteLine("Number: {0}", MyDynamic.number);
            MyDynamic.Increment();
            Console.WriteLine("Number: {0}", MyDynamic.number);
            Console.WriteLine("**********************************");



            Console.WriteLine("**********************************");

            dynamic person = new DynamicDictionary();

            person.FirstName = "Mustafa";
            person.LastName = "Sacli";
            person.Method = new Func<string>(() => { return "Empty method"; });
            Console.WriteLine("Person: {0} {1}", person.FirstName, person.LastName);

            Console.WriteLine("Property count: {0}", person.Count);

            Console.WriteLine("Table Name: {0}", person.GetTable());

            Console.WriteLine("Virtual Key Column: {0}", person.GetVirtualKey());

            foreach (var item in person.GetAllMembers())
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            }

            foreach (var item in person.GetDynamicMemberNames())
            {
                object o;
                if (person.Get(item, out o))
                {
                    Console.WriteLine("{0} : {1}", item, o);
                }
            }

            Console.WriteLine("{0}", person.Method());

            person.SetVirtualKey("ObjId");
            person.SetTable("New Table");
            person.SetSchema("NSchem");

            Console.WriteLine("Virtual Key: {0}", person.GetVirtualKey());
            Console.WriteLine("Table Name : {0}", person.GetTable());
            Console.WriteLine("Schema : {0}", person.GetSchema());


            DynamicDictionary.Current.IsUser = true;
            DynamicDictionary.Current.UserName = "Alkun";
            DynamicDictionary.Current.FirstName = "Musti";
            DynamicDictionary.Current.LastName = "Huylu";
            DynamicDictionary.Current.BirthDate = DateTime.Parse("19.05.1987");


            foreach (var item in DynamicDictionary.Current.GetAllMembers())
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            }

            Console.ReadKey();
        }
    }

    public class DynamicDictionary : DynamicObject
    {
        private static DynamicDictionary mCurrent = new DynamicDictionary();

        public static dynamic Current
        {
            get { return DynamicDictionary.mCurrent; }
        }



        private Dictionary<string, object> members = new Dictionary<string, object>();
        private string tableName = string.Empty;
        private string schemaName = string.Empty;
        private string virtualKeyColumn = string.Empty;

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            try
            {
                //return base.GetDynamicMemberNames();
                return ((IEnumerable<string>)(from s in members select s.Key).ToList<string>());
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal IDictionary<string, object> GetAllMembers()
        {
            return members;
        }


        /// <summary>
        /// Gets Count.
        /// </summary>
        public int Count { get { return members.Count; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Get(string memberName, out object value)
        {
            bool result = false;
            try
            {
                value = null;
                if (members.ContainsKey(memberName))
                {
                    value = members[memberName];
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public bool Remove(string memberName)
        {
            bool result = false;
            try
            {
                if (members.ContainsKey(memberName))
                {
                    members.Remove(memberName);
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public bool Set(string memberName, object value)
        {
            bool result = false;
            try
            {
                if (members.ContainsKey(memberName))
                {
                    members[memberName] = value;
                    result = true;
                }
                else
                {
                    members.Add(memberName, value);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                string name = binder.Name;//.ToLower();
                return members.TryGetValue(name, out result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                string name = binder.Name;//.ToLower();
                members[name] = value;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;
            bool resBool = false;
            try
            {
                if (members.ContainsKey(binder.Name) && members[binder.Name] is Delegate)
                {
                    result = (members[binder.Name] as Delegate).DynamicInvoke(args);
                    resBool = true;
                }
                else
                {
                    return base.TryInvokeMember(binder, args, out result);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return resBool;
        }

        public string GetVirtualKey() { return this.virtualKeyColumn; }

        public string GetTable() { return this.tableName; }

        public string GetSchema() { return this.schemaName; }

        public void SetTable(string tableName) { this.tableName = tableName; }

        public void SetSchema(string schemaName) { this.schemaName = schemaName; }

        public void SetVirtualKey(string virtualKeyColumn) { this.virtualKeyColumn = virtualKeyColumn; }
    }


}
