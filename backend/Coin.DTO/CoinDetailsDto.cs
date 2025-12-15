using Base.DTO;

namespace Coin.DTO
{
    public class CoinDetailsDto : BaseDto<int>
    {
        public new int Id { get; set; }
        public string Symbol { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string UrlIcon { get; set; } = null!;
        public DateTime GenesisDate { get; set; }
        public string Description { get; set; } = null!;
        public float Prices { get; set; }
        public float VolumeTraded { get; set; }
    }
}
