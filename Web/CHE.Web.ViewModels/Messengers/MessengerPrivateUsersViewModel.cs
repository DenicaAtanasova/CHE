namespace CHE.Web.ViewModels.Messengers
{
    using System.Collections.Generic;

    public class MessengerPrivateUsersViewModel
    {
        public IEnumerable<MessengerUserViewModel> Users { get; set; }

        public MessengerPrivateViewModel CurrentMessenger { get; set; }
    }
}