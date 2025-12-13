using Newtonsoft.Json;

namespace Сoin.Entity.JSON
{
    public partial class Image
    {
        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }
}
