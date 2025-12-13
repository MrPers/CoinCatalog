using AutoMapper;
using Letter.Api.Models;
using Letter.DTO;
using Letter.Entity.DB;
using Letter.Entity.JSON;

namespace Letter
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<LetterEntity, LetterDto>().ReverseMap();
            CreateMap<LetterDto, LetterVM>().ReverseMap();

            CreateMap<CoinRateJSON, CoinRateDto>().ReverseMap();
        }
    }

}
