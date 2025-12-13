using AutoMapper;
using Сoin.Api.Models;
using Сoin.Entity;
using Сoin.DTO;
using System;
using Сoin.Entity.DB;
using Сoin.Entity.JSON;

namespace Сoin
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<CoinRateDto, CoinRateVMInTicks>()
                .ForMember(dst => dst.Time, opt => opt.MapFrom(src => GetJavascriptTimestamp(src.Time)));
            CreateMap<CoinRateDto, CoinRateVMInDateTime>();
            CreateMap<CoinRate, CoinRateDto>().ReverseMap();
            CreateMap<CoinRateJSON, CoinRateDto>()
                .ForMember(dst => dst.Prices, opt => opt.MapFrom(src => (src.PriceHigh + src.PriceLow)/2));

            CreateMap<CoinJSON, CoinDto>()
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description.En))
                .ForMember(dst => dst.UrlIcon, opt => opt.MapFrom(src => src.UrlIcon.Thumb));
            CreateMap<Coin, CoinDto>().ReverseMap();
            CreateMap<CoinDto, CoinVM>();
            CreateMap<CoinDto, CoinFullVM>();
        }

        static long GetJavascriptTimestamp(DateTime input)
        {
            var span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            var time = input.Subtract(span);
            return (long)(time.Ticks / 10000);
        }
    }
}
