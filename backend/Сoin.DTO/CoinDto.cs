using Base.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сoin.DTO
{
    public class CoinDto : BaseDto<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlIcon { get; set; }
        public DateTime GenesisDate { get; set; }
        public string Description { get; set; }
        public float Prices { get; set; }
        public float VolumeTraded { get; set; }
    }
}
