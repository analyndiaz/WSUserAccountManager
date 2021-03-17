using System;

namespace WSUserAccountManager.Database.Entities
{
    public class Session : CreatedInstance
    {
        public int SessionId { get; set; }

        public int UserAccountId { get; set; }

        public string Value { get; set; }

        public int Validity { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}