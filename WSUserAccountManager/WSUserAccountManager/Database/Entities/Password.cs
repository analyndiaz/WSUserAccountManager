namespace WSUserAccountManager.Database.Entities
{
    public class Password : CreatedInstance
    {
        public int PasswordId { get; set; }

        public int UserAccountId { get; set; }

        public string Value { get; set; }

        public bool IsPrimary { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}