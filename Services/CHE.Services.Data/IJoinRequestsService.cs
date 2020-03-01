﻿namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<bool> SendAsync(string requestContent, string cooperativeId, string receiverId, string senderId);

        Task<bool> AcceptAsync(string requestId);

        Task<bool> RejectAsync(string requestId);
    }
}