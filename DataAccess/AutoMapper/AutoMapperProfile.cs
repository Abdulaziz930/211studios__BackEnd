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

            CreateMap<Studio, StudioDto>().ReverseMap();

            CreateMap<Bio, BioDto>().ReverseMap();

            CreateMap<Bio, BioContactDto>().ReverseMap();

            CreateMap<Social, SocialDto>().ReverseMap();

            CreateMap<Studio, StudioPageDto>()
                .ForMember(x => x.BannerTitle, y => y.MapFrom(x => x.Banner.Title))
                .ForMember(x => x.BannerDescription, y => y.MapFrom(x => x.Banner.Description))
                .ForMember(x => x.BannerImage, y => y.MapFrom(x => x.Banner.Image)).ReverseMap();

            CreateMap<UserSocialMedia, UserSocialMediaDto>().ReverseMap();

            CreateMap<AppUser, AppUserDto>().ReverseMap();

            CreateMap<TeamMemberBanner, TeamMemberBannerDto>().ReverseMap();
        }
    }
}
