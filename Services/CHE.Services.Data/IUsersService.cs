namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<bool> SendJoinRequestAsync(string requestContent, string cooperativeId, string receiverId, string senderName);

        Task<bool> AcceptRequestAsync(string requestId);

        Task<bool> RejectRequestAsync(string requestId);

        Task<bool> RemoveMemberFromCooperativeAsync(string memberId, string cooperativeId);
    }
}