using System.ComponentModel.DataAnnotations;

namespace Сoin.Api.Models
{
    public class CoinRateQuestion
    {
        public int Id { get; set; }
        public int Step { get; set; }
        public bool InTick { get; set; } = true;
    }
}
