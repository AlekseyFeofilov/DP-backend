using Newtonsoft.Json;

namespace DP_backend.Models.DTOs.TSUAccounts
{
    public class TSUAuthResponseDTO
    {
        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("AccountId")]
        public Guid AccountId { get; set; }
    }
}
