using Letter.DTO;

namespace Letter.Contracts.Business
{
    public interface ILetterBusiness
    {
        String MakingFile(List<CoinRateDto> coins);
    }
}
