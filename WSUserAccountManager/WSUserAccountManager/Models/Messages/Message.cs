using Newtonsoft.Json;

namespace WSUserAccountManager.Models.Messages
{
    public class Message
    {
        [JsonProperty("command")]
        public string Command { get; set; }
    }
}