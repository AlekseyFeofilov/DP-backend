using Newtonsoft.Json;

namespace DP_backend.Models.DTOs.TSUAccounts
{
    public class TSUAuthRequestDTO
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty("secretKey")]
        public string SecreteKey { get; set; }
    }
}
