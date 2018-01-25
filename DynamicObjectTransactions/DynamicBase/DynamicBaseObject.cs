using System;
using System.Collections.Generic;
using System.Dynamic;

namespace DynamicBase
{
    public class DynamicBaseObject : DynamicObject, IDynamicBaseObject
    {
        private Dictionary<string, object> members = new Dictionary<string, object>();
        private string tableName = string.Empty;
        private string schemaName = string.Empty;
        private string virtualKeyColumn = string.Empty;


        internal IDictionary<string, object> GetAllMembers()
        {
            return members;
        }

        /// <summary>
        /// Gets Count.
        /// </summary>
        public int Count { get { return members.Count; } }

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
        public string GetVirtualKey()
        {
            return this.virtualKeyColumn;
        }

        public string GetTable()
        {
            return this.tableName;
        }

        public string GetSchema()
        {
            return this.schemaName;
        }

        public void SetTable(string tableName)
        {
            this.tableName = tableName;
        }

        public void SetSchema(string schemaName)
        {
            this.schemaName = schemaName;
        }

        public void SetVirtualKey(string virtualKeyColumn)
        {
            this.virtualKeyColumn = virtualKeyColumn;
        }

        public virtual string InsertQuery()
        {
            string result = string.Empty;
            try
            {
                result = "INSERT INTO ";
                if (!string.IsNullOrWhiteSpace(schemaName))
                {
                    result = string.Format("{0}[{1}].", result, schemaName);
                }


                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    result = string.Format("{0}[{1}](", result, tableName);
                }

                members.Remove(virtualKeyColumn);
                foreach (var item in members.Keys)
                {
                    result = string.Format("{0} {1}, ", result, item);
                }

                result = result.TrimEnd().TrimEnd(',');

                result = string.Format("{0}) VALUES(", result);

                foreach (object item in members.Values)
                {
                    if (item == null)
                    {
                        result = string.Format("{0} {1}, ", result, "NULL");
                        continue;
                    }

                    if (string.Equals(item.GetType().Name, "String"))
                    {
                        result = string.Format("{0} N'{1}', ", result, string.Format("{0}", item).Replace("'", "''"));
                        continue;
                    }

                    if (string.Equals(item.GetType().Name, "Boolean"))
                    {
                        result = string.Format("{0} {1}, ", result, (bool)item ? 1 : 0);
                        continue;
                    }

                    if (string.Equals(item.GetType().Name, "Int16") || string.Equals(item.GetType().Name, "Int32") || string.Equals(item.GetType().Name, "Int64") || string.Equals(item.GetType().Name, "Byte"))
                    {
                        result = string.Format("{0} {1}, ", result, item);
                        continue;
                    }
                }

                result = result.TrimEnd().TrimEnd(',');

                result = string.Format("{0});", result);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

    }
}