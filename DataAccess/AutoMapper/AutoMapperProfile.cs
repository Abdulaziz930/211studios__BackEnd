using AutoMapper;
using Entities.DTOs;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Slider, SliderDto>().ReverseMap();

            CreateMap<Game, GameDto>().ReverseMap();

            CreateMap<GameDetail, GameDetailDto>().ReverseMap();
            
            CreateMap<Category, CategoryDto>().ReverseMap();
            
            CreateMap<Platform, PlatformDto>().ReverseMap();   

            CreateMap<Studio, StudioDetailDto>().ForMember(x => x.IntroDescription, y => y.MapFrom(x => x.StudioDetail.IntroDescription))
                .ForMember(x => x.DetailImage, y => y.MapFrom(x => x.StudioDetail.DetailImage)).ReverseMap();

            CreateMap<Studio, StudioDto>().ReverseMap();
        }
    }
}
