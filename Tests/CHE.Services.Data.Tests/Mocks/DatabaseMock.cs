namespace CHE.Services.Data.Tests.Mocks
{
    using CHE.Data;

    using Microsoft.EntityFrameworkCore;

    using System;

    public class DatabaseMock
    {
        public static CheDbContext Instance =>
            new CheDbContext(
                new DbContextOptionsBuilder<CheDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);
        
    }
}