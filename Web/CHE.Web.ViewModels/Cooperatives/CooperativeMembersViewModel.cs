namespace CHE.Web.ViewModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.Collections.Generic;

    public class CooperativeMembersViewModel : IMapExplicitly
    {
        public string Id { get; init; }


        public string Admin { get; init; }

        public IEnumerable<CooperativeUserDetailsViewModel> Members { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeMembersViewModel>()
                .ForMember(dest => dest.Admin, opt =>
                    opt.MapFrom(src => src.Admin.User.UserName));
        }
    }
}