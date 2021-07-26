namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task<string> CreateAsync(
            string senderId,
            string receiverId,
            string comment,
            int rating);

        Task UpdateAsync(string id, string comment, int rating);

        Task DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllByReceiverAsync<TEntity>(string receiverId);

        Task<string> GetSentReviewIdAsync(string receiverId, string senderId);
    }
}