using System.ComponentModel.DataAnnotations;

namespace Coin.Api.Models
{
    public class CoinPriceVMInTicks
    {
        public double Prices { get; set; }
        public double VolumeTraded { get; set; }
        public long Time { get; set; }
    }
}
