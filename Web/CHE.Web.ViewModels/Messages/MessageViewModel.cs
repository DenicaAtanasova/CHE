namespace CHE.Web.ViewModels.Messages
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class MessageViewModel : IMapExplicitly
    {
        public string Sender { get; set; }

        public string Text { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(dest => dest.CreatedOn, opt =>
                    opt.MapFrom(src =>
                        src.CreatedOn.ToLocalTime()));
        }
    }
}