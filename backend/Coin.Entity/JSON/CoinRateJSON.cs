using Newtonsoft.Json;

namespace Coin.Entity.JSON
{
    public class CoinRateJSON
    {
        [JsonProperty("prices")]
        public List<List<double>> Prices { get; set; }

        [JsonProperty("market_caps")]
        public List<List<double>> MarketCaps { get; set; }

        [JsonProperty("total_volumes")]
        public List<List<double>> TotalVolumes { get; set; }
    }
}
