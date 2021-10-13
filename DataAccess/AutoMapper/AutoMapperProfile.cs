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
        }
    }
}
