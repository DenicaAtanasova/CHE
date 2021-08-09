namespace CHE.Web.ViewModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeAllViewModel : IMapExplicitly
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string Grade { get; init; }

        public string Info { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeAllViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.ToString()));
        }
    }
}