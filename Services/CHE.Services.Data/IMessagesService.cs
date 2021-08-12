namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        Task<string> CreateAsync(string messengerId, string senderName, string message);

        Task<TEntity> GetByIdAsync<TEntity>(string id);
    }
}