namespace CHE.Web.ViewModels.Messengers
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.Collections.Generic;

    public class MessengerCooperativeUsersViewModel : IMapFrom<Messenger>
    {
        public string CooperativeId { get; set; }

        public string CooperativeName { get; set; }

        public IEnumerable<MessengerUserViewModel> Users { get; set; }
    }
}