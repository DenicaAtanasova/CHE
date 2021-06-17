﻿namespace CHE.Services.Data
{
    using CHE.Data.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGradesService
    {
        Task<string> GetIdAsync(string value);

        Task<IEnumerable<string>> GetAllValuesAsync(string currentGrade = null);
    }
}