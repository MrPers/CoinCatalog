using Base.DTO;
using Newtonsoft.Json;

namespace Letter.DTO
{
    public class CoinRateDto
    {
        public double Prices { get; set; }
        public double VolumeTraded { get; set; }
        public DateTime? Time { get; set; }
    }
}
