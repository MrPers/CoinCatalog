using Letter.DTO;

namespace Letter.Contracts.Persistence
{
    public interface ILetterAPI
    {
        Task SendLetterAsync(LetterDto letters, string filePath);
        Task<List<CoinRateDto>> GetCoinsRateDtoById(int IdCoin, int StepCoin);
    }
}
