using Newtonsoft.Json;

namespace Coin.Entity.JSON
{
    public class CoinDetailsJSON
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("description")]
        public Description Description { get; set; }
        [JsonProperty("image")]
        public Image UrlIcon { get; set; }
        [JsonProperty("genesis_date")]
        public DateTime? GenesisDate { get; set; }
    }
}
