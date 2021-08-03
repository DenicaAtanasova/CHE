namespace CHE.Web.ViewModels.Reviews
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class ReviewAllViewModel : IMapExplicitly
    {
        public string Id { get; init; }

        public string Comment { get; init; }

        public int Rating { get; init; }

        public string Sender { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewAllViewModel>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender.User.UserName));
        }
    }
}