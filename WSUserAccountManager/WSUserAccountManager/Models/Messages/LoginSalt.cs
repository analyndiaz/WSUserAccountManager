using Newtonsoft.Json;

namespace WSUserAccountManager.Models.Messages
{
    public class LoginSalt : Message
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}