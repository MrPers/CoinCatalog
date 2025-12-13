using Base.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Сoin.Entity.DB
{
    public class Coin : BaseEntity<int>
    {
        public string Name { get; set; }
        public DateTime? GenesisDate { get; set; }
        public string Description { get; set; }
        public string UrlIcon { get; set; }
        public ICollection<CoinRate> CoinRate { get; set; } = new List<CoinRate>();
    }

}
