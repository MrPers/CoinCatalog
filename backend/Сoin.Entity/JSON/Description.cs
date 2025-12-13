using Newtonsoft.Json;

namespace Сoin.Entity.JSON
{
    public partial class Description
    {
        [JsonProperty("en")]
        public string En { get; set; }
    }
}
