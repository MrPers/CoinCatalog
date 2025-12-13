using Base.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Сoin.Entity.DB
{
    public class CoinRate : BaseEntity<int>
    {
        public int CoinId { get; set; }
        [ForeignKey("CoinId")]
        [Required]
        public Coin? Coin { get; set; }
        public DateTime Time { get; set; }
        public float VolumeTraded { get; set; }
        public float Prices { get; set; }

    }
}