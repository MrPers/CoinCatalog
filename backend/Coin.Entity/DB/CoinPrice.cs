using Base.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coin.Entity.DB
{
    public class CoinPrice : BaseEntity<int>
    {
        public int CoinId { get; set; }
        [ForeignKey("CoinId")]
        [Required]
        public CoinDetails? Coin { get; set; }
        public DateTime Time { get; set; }
        public float VolumeTraded { get; set; }
        public float Prices { get; set; }

    }
}