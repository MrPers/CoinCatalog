using Newtonsoft.Json;

namespace Coin.Entity.JSON
{
    public partial class Image
    {
        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }
}
