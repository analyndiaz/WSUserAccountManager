using System.Collections.Generic;

namespace WSUserAccountManager.Database.Entities
{
    public class UserAccount : CreatedInstance
    {
        public int UserAccountId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public virtual IEnumerable<Password> Passwords { get; set; }

        public virtual IEnumerable<VerificationCode> VerificationCodes { get; set; }

        public virtual IEnumerable<Salt> Salts { get; set; }

        public virtual IEnumerable<Session> Sessions { get; set; }
    }
}