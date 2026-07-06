using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MTGProject.Models;

namespace MTGProject.Services
{
    public class FilterLocalStorageService
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private const string Key = "mtg_filters";

        public FilterLocalStorageService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task SaveFiltersAsync(FilterData filters, String key)
        {
            await _sessionStorage.SetAsync(key, filters);
        }

        public async Task<FilterData> LoadFiltersAsync(String key)
        {
            var result = await _sessionStorage.GetAsync<FilterData>(key);
            return result.Success ? result.Value! : new FilterData();
        }

        public async Task ClearFiltersAsync()
        {
            await _sessionStorage.DeleteAsync(Key);
        }
    }
}
