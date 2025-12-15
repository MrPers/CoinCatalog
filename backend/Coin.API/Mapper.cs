using AutoMapper;
using Coin.Api.Models;
using Coin.Entity;
using Coin.DTO;
using System;
using Coin.Entity.DB;
using Coin.Entity.JSON;

namespace Coin
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            //CreateMap<CoinPriceDto, CoinPriceVMInTicks>();
                //.ForMember(dst => dst.Time, opt => opt.MapFrom(src => GetJavascriptTimestamp(src.Time)));
            CreateMap<CoinPriceDto, CoinPriceVMInDateTime>().ReverseMap();
            CreateMap<CoinPrice, CoinPriceDto>().ReverseMap();
            //CreateMap<CoinRateJSON, CoinRateDto>()
            //    .ForMember(dst => dst.Prices, opt => opt.MapFrom(src => (src.PriceHigh + src.PriceLow)/2));

            CreateMap<CoinDetailsJSON, CoinDetailsDto>()
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description.En))
                .ForMember(dst => dst.UrlIcon, opt => opt.MapFrom(src => src.UrlIcon.Thumb));
            CreateMap<CoinDetails, CoinDetailsDto>().ReverseMap();
            CreateMap<CoinDetailsDto, CoinDetailsVM>();
            CreateMap<CoinDetailsDto, CoinFullDetailsVM>();
        }

        //static long GetJavascriptTimestamp(DateTime input)
        //{
        //    var span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
        //    var time = input.Subtract(span);
        //    return (long)(time.Ticks / 10000);
        //}
    }
}
