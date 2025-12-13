using AutoMapper;
using Base.Data;
using Letter.Contracts.Repo;
using Letter.DTO;
using Letter.Entity.DB;

namespace Letter.Data.Repository
{
    public class LetterRepository : BaseDataContext<LetterEntity, LetterDto, int>, ILetterRepository
    {
        public LetterRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {

        }

    }
}