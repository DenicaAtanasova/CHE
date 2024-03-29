﻿namespace CHE.Web.ViewModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeDetailsViewModel : IMapExplicitly
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string Info { get; init; }

        public string Grade { get; init; }

        public int MembersCount { get; init; }

        public string Admin { get; init; }

        public string AdminId { get; init; }

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }

        public string PendingRequestId { get; set; }

        public CooperativeAddressViewModel Address { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeDetailsViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.ToString()))
                .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.Admin.User.UserName))
                .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.Admin.User.Id));
        }
    }
}