namespace CHE.Web.Cache
{
    using CHE.Common.Extensions;
    using CHE.Services.Data;

    using Microsoft.Extensions.Caching.Memory;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AddressCache : IAddressCache
    {
        private const string CitiesCacheKey = "citiesCacheKey";
        private const string NeighbourhoodsCacheKey = "neighbourhoodsCacheKey";

        private readonly IMemoryCache _cache;
        private readonly IAddressesService _addressesService;

        public AddressCache(IMemoryCache cache, IAddressesService addressesService)
        {
            this._cache = cache;
            this._addressesService = addressesService;
        }

        public void Set(string city, string neighbourhood)
        {
            this.SetCache(CitiesCacheKey, city);
            this.SetCache(NeighbourhoodsCacheKey, neighbourhood);
        }

        public async Task<IEnumerable<string>> GetAsync(CacheType cacheType) =>
        cacheType switch
        {
            CacheType.City => await this.GetCityCache(),
            CacheType.Neighbourhood => await this.GetNeighbourhoodCache(),
            _ => null
        };

        private void SetCache(string cacheKey, string cacheItem)
        {
            var cachValue = this._cache.Get<IList<string>>(cacheKey);
            var cachItemTitleCase = cacheItem.ToTitleCase();

            if (!cachValue.Contains(cachItemTitleCase))
            {
                cachValue.Add(cachItemTitleCase);
                _cache.Set(
                    cacheKey,
                    cachValue);
            }
        }

        private async Task<IEnumerable<string>> GetCityCache()
        {
                var cities = this._cache.Get<IEnumerable<string>>(CitiesCacheKey);

                if (cities == null)
                {
                    cities = await this._addressesService.GetAllCitiesAsync();
                    this._cache.Set(CitiesCacheKey, cities);
                }

                return cities;
        }

        private async Task<IEnumerable<string>> GetNeighbourhoodCache()
        {
            var neighbourhoods = this._cache.Get<IEnumerable<string>>(NeighbourhoodsCacheKey);

            if (neighbourhoods == null)
            {
                neighbourhoods = await this._addressesService.GetAllNeighbourhoodsAsync();
                this._cache.Set(NeighbourhoodsCacheKey, neighbourhoods);
            }

            return neighbourhoods;
        }
    }

    public enum CacheType
    {
        City = 0,
        Neighbourhood = 1
    }
}