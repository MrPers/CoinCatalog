using System.ComponentModel.DataAnnotations;

namespace Coin.Api.Models
{
    public class CoinRateQuestion
    {
        public int Id { get; set; }
        public int Step { get; set; }
        public bool InTick { get; set; } = true;
    }
}
