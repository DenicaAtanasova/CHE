namespace CHE.Web.ViewModels.Messengers
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.ViewModels.Messages;

    using System.Collections.Generic;
    using System.Linq;

    public class MessengerCooperativeViewModel : IMapExplicitly
    {
        public string Id { get; set; }

        public ICollection<MessageViewModel> Messages { get; set; }

        public string CurrentUser { get; set; }

        public string CooperativeName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Messenger, MessengerCooperativeViewModel>()
                .ForMember(dest => dest.Messages, opt =>
                    opt.MapFrom(src => src.Messages.OrderBy(x => x.CreatedOn)));
        }
    }
}
