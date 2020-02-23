﻿namespace CHE.Services.Mapping
{
    using AutoMapper;
    using CHE.Data.Models;
    using CHE.Web.ViewModels.Cooperatives;

    public class CooperativeProfile : Profile
    {
        public CooperativeProfile()
        {
            this.CreateMap<Cooperative, CooperativeAllViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value));
        }
    }
}