using AgricFameAPICosmosDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgricFameAPICosmosDB.Services.Interfaces
{
    public interface IFarmCosmosDbService
    {
        Task<IEnumerable<Farm>> GetFarmsAsync(string query);
        Task<Farm> GetFarmAsync(string id);

        Task AddFarmAsync(Farm farm);

        Task UpdateFarmAsync(string id, Farm farm);

        Task DeleteAsync(string id);
    }
}