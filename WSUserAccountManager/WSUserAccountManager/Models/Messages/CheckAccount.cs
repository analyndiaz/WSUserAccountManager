using Newtonsoft.Json;

namespace WSUserAccountManager.Models.Messages
{
    public class CheckAccount : LoginSalt
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}