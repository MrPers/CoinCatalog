using Newtonsoft.Json;

namespace Сoin.Entity.JSON
{
    public class CoinRateJSON
    {
        [JsonProperty("time_period_end")]
        public DateTime Time;
        [JsonProperty("price_high")]
        public float PriceHigh;
        [JsonProperty("price_low")]
        public float PriceLow;
        [JsonProperty("volume_traded")]
        public float VolumeTraded;
    }
}
