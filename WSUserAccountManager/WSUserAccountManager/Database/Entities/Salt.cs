using System;

namespace WSUserAccountManager.Database.Entities
{
    public class Salt : CreatedInstance
    {
        public int SaltId { get; set; }

        public int UserAccountId { get; set; }

        public string Value { get; set; }

        public int Validity { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}