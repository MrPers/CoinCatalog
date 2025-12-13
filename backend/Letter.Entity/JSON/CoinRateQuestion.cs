using System.ComponentModel.DataAnnotations;

namespace Letter.Entity.JSON
{
    public class CoinRateQuestion
    {
        public CoinRateQuestion(int id, int step)
        {
            Id = id;
            Step = step;
        }

        public int Id { get; }
        public int Step { get; }
        public bool InTick { get; } = false;
    }
}
