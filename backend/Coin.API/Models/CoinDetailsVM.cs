using System.ComponentModel.DataAnnotations;

namespace Coin.Api.Models
{
    public class CoinDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public string UrlIcon { get; set; } = null!;
        public float Prices { get; set; }
        public float VolumeTraded { get; set; }
    }
}
