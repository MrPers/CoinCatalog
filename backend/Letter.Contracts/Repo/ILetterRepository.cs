using Letter.DTO;
using Base.Contracts;
using Letter.Entity.DB;

namespace Letter.Contracts.Repo
{
    public interface ILetterRepository : IBaseRepository<LetterEntity, LetterDto, int>
    {
    }
}

