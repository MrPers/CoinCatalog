using Newtonsoft.Json;

namespace Letter.Entity.JSON
{
    public class CoinRateJSON
    {
        [JsonProperty("prices")]
        public double Prices { get; set; }
        [JsonProperty("volumeTraded")]
        public double VolumeTraded { get; set; }
        [JsonProperty("time")]
        public DateTime? Time { get; set; }
    }
}
