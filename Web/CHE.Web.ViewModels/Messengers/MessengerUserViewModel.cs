namespace CHE.Web.ViewModels.Messengers
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class MessengerUserViewModel : IMapFrom<MessengerUser>
    {
        public string UserId { get; set; }

        public string UserUserName { get; set; }
    }
}