using Newtonsoft.Json;

namespace WSUserAccountManager.Models.Messages
{
    public class Login : Message
    {
        [JsonProperty("usernameOrEmail")]
        public string UsernameOrEmail { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }
    }
}