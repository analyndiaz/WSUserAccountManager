namespace WSUserAccountManager.Models
{
    public class UserAccount
    {
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public string SecondaryPassword { get; set; }

        public string Email { get; set; }

        public string VerificationCode { get; set; }
    }
}