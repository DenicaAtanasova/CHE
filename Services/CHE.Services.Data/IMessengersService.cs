namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessengersService
    {
        Task<TEntity> GetCooperativeMessengerAsync<TEntity>(string cooperativeId);

        Task<string> GetPrivateMessengerIdAsync(string senderId, string receiverId);

        Task<TEntity> GetPrivateMessengerAsync<TEntity>(string senderId, string receiverId);

        Task<IEnumerable<TEntity>> GetAllPrivateContactsByUserAsync<TEntity>(string userId);

        Task AddMemberAsync(string messengerId, string userId);

        Task RemoveMemberAsync(string messengerId , string userId);
    }
}