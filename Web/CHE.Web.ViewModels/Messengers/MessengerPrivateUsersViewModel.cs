namespace CHE.Web.ViewModels.Messengers
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.Collections.Generic;

    public class MessengerPrivateUsersViewModel : IMapFrom<Messenger>
    {
        public IEnumerable<MessengerUserViewModel> Users { get; set; }
    }
}