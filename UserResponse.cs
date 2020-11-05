using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureGraphApiTry
{
    public class UserResponse
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }
        [JsonProperty("value")]
        public List<User> Users { get; set; }
    }
}
