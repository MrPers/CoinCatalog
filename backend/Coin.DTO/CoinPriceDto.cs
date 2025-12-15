using Base.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.DTO
{
    public class CoinPriceDto : BaseDto<int>
    {
        public new int Id { get; set; }
        public double MarketCap { get; set; }
        public double TotalVolume { get; set; }
        public double Price { get; set; }
        public DateTime Time { get; set; }
        public int CoinId { get; set; }
    }
}
