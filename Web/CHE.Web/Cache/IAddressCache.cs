namespace CHE.Web.Cache
{

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAddressCache
    {
        void Set(string city, string neighbourhood);

        Task<IEnumerable<string>> GetAsync(CacheType cacheType);

    }

}