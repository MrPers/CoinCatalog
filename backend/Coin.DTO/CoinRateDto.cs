using Base.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin.DTO
{
    public class CoinRateDto : BaseDto<int>
    {
        public int Id { get; set; }
        public List<DateTime> Time { get; set; } = new List<DateTime>();
        public List<double> Prices { get; set; } = new List<double>();
        public List<double> MarketCaps { get; set; } = new List<double>();
        public List<double> TotalVolumes { get; set; } = new List<double>();
        //public double Prices { get; set; }
        //public double VolumeTraded { get; set; }
        //public DateTime Time { get; set; }
        //public int CoinId { get; set; }
    }
}
