
namespace DynamicBase
{
    struct EntityPropertyChange
    {

        public int EntityChangeId { get; set; }

        public string ColumnName { get; set; }

        public object OldValue { get; set; }

        public object NewValue { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is EntityChange)
            {
                EntityPropertyChange epc = (EntityPropertyChange)obj;
                result = string.Equals(epc.ColumnName, this.ColumnName);
            }

            return result;
        }

    }
}
