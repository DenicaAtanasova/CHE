namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task<bool> CreateAsync(string comment, int rating, string senderId, string receiverId);

        Task<IEnumerable<TEntity>> GetTeachersAllAsync<TEntity>(string teacherId);
    }
}