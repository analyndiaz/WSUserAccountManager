using System;

namespace WSUserAccountManager.Database.Entities
{
    public class CreatedInstance
    {
        public DateTime CreatedTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedBy { get; set; }

    }
}