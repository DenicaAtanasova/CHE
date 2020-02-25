namespace CHE.Services.Data
{
    using CHE.Data.Models;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<bool> AcceptRequest(string requestId);

        Task<bool> RejectRequest(string requestId);
    }
}