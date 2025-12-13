using Base.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сoin.DTO
{
    public class CoinRateDto : BaseDto<int>
    {
        public int Id { get; set; }
        public double Prices { get; set; }
        public double VolumeTraded { get; set; }
        public DateTime Time { get; set; }
        public int CoinId { get; set; }
    }
}
