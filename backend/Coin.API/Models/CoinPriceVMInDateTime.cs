using System;
using System.ComponentModel.DataAnnotations;

namespace Coin.Api.Models
{
    public class CoinPriceVMInDateTime
    {
        public double Prices { get; set; }
        public double VolumeTraded { get; set; }
        public DateTime Time { get; set; }
    }
}
