
using Newtonsoft.Json;

namespace KLG.Backend.Promotion.Models.Request
{
    public class CreateTokenDto
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}
