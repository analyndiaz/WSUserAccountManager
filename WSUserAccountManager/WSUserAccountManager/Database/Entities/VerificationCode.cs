namespace WSUserAccountManager.Database.Entities
{
    public class VerificationCode : CreatedInstance
    {
        public int VerificationCodeId { get; set; }

        public int UserAccountId { get; set; }

        public string Code { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}