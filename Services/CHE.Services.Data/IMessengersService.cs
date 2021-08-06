namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessengersService
    {
        Task<string> GetPrivateMessengerIdAsync(string senderId, string receiverId);

        Task<TEntity> GetCooperativeMessengerWithUsersAsync<TEntity>(string cooperativeId);

        Task<TEntity> GetCooperativeMessengerWithMessagesAsync<TEntity>(string cooperativeId);

        Task<IEnumerable<TEntity>> GetAllPrivateMessengersByUserAsync<TEntity>(string userId);

        Task<TEntity> GetPrivateMessengerAsync<TEntity>(string senderId, string receiverId);
    }
}