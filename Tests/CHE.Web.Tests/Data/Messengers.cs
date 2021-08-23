namespace CHE.Web.Tests.Data
{
    using CHE.Web.ViewModels.Messengers;
    using System.Collections.Generic;

    public class Messengers
    {
        public static IEnumerable<MessengerUserViewModel> AllPrivateMessengers =>
            new List<MessengerUserViewModel>
            {
                new MessengerUserViewModel(),
                new MessengerUserViewModel(),
                new MessengerUserViewModel(),
            };

        public static MessengerPrivateViewModel CurrentMessenger =>
            new MessengerPrivateViewModel
            {
                CurrentReceiver = "receiverId"
            };

        public static MessengerPrivateUsersViewModel MessengerUsers =>
            new MessengerPrivateUsersViewModel
            {
                Users = AllPrivateMessengers,
                CurrentMessenger = CurrentMessenger
            };
    }
}
