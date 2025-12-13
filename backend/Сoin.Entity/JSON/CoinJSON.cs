using Newtonsoft.Json;

namespace Сoin.Entity.JSON
{
    public class CoinJSON
    {
        [JsonProperty("symbol")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public Description Description { get; set; }
        [JsonProperty("image")]
        public Image UrlIcon { get; set; }
        [JsonProperty("genesis_date")]
        public DateTime? GenesisDate { get; set; }
    }
}
