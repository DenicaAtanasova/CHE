namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<bool> SendJoinRequest(string requestContent, string cooperativeId, string receiverId, string senderName);

        Task<bool> AcceptRequest(string requestId);

        Task<bool> RejectRequest(string requestId);
    }
}