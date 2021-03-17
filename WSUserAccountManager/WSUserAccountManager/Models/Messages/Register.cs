using Newtonsoft.Json;

namespace WSUserAccountManager.Models.Messages
{
    public class Register : CheckAccount
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("password2")]
        public string SecondaryPassword { get; set; }

        [JsonProperty("verificationCode")]
        public string VerificationCode { get; set; }
    }
}