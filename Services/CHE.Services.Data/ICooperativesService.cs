﻿namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICooperativesService
    {
        Task<bool> CreateAsync(string name, string info, string gradeValue, string creatorName);

        Task<bool> UpdateAsync(string id, string name, string info, string gradeValue, string city, string neighbourhood, string street = null);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        Task<ICollection<TEntity>> GetJoinRequestsAsync<TEntity>(string cooperativeId);

        Task SendTeacherRequest(string cooperativeId, string teacherId);

        Task RemoveTeacherRequest(string cooperativeId, string teacherId);
    }
}