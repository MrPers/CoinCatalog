using System;
using System.ComponentModel.DataAnnotations;

namespace Сoin.Api.Models
{
    public class CoinRateVMInDateTime
    {
        public double Prices { get; set; }
        public double VolumeTraded { get; set; }
        public DateTime Time { get; set; }
    }
}
