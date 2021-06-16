namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICheUsersService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(int startIndex, int endIndex, string schoolLevelFilter);

        Task<int> Count(string schoolLevelFilter);

        Task AcceptRequestAsync(string requestId);

        Task RejectRequestAsync(string requestId);
    }
}