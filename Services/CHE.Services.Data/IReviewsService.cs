namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Reviews;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task<string> CreateAsync(string senderId, ReviewCreateInputModel inputModel);

        Task UpdateAsync(ReviewUpdateInputModel inputModel);

        Task DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllByReceiverAsync<TEntity>(string receiverId);
    }
}