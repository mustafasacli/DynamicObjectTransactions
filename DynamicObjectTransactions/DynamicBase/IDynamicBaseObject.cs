using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBase
{
    interface IDynamicBaseObject
    {
        string GetVirtualKey();

        string GetTable();

        string GetSchema();

        void SetTable(string tableName);

        void SetSchema(string schemaName);

        void SetVirtualKey(string virtualKeyColumn);
    }
}
