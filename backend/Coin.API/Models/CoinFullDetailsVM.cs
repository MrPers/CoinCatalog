using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coin.Api.Models
{
    public class CoinFullDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string UrlIcon { get; set; }
        public DateTime GenesisDate { get; set; }
        public string Description { get; set; }
    }
}
