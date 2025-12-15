using Base.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coin.Entity.DB
{
    public class CoinDetails : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public DateTime? GenesisDate { get; set; }
        public string Description { get; set; } = null!;
        public string UrlIcon { get; set; } = null!;
        public ICollection<CoinPrice> CoinPrices { get; set; } = new List<CoinPrice>();
    }

}
