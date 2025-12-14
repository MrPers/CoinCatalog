using Newtonsoft.Json;

namespace Coin.Entity.JSON
{
    public partial class Description
    {
        [JsonProperty("en")]
        public string En { get; set; }
    }
}
