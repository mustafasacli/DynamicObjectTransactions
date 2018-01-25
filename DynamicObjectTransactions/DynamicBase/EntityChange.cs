using System;

namespace DynamicBase
{
    class EntityChange
    {
        public int Id { get; set; }

        public string EntityName { get; set; }

        public int UserId { get; set; }

        public string ChangeType { get; set; }

        public DateTime ChangedOn { get; set; }

        public DateTime TransactionTime { get; set; }
    }
}
