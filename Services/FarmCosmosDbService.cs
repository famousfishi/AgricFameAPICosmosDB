using AgricFameAPICosmosDB.Models;
using AgricFameAPICosmosDB.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgricFameAPICosmosDB.Services
{
    public class FarmCosmosDbService : IFarmCosmosDbService
    {
        private Container _container;

        public FarmCosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddFarmAsync(Farm farm)
        {
            await _container.CreateItemAsync(farm, new PartitionKey(farm.Id));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<Farm>(id, new PartitionKey(id));
        }

        public async Task<Farm> GetFarmAsync(string id)
        {
            try
            {
                ItemResponse<Farm> response = await _container.ReadItemAsync<Farm>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                //if farm data not found
                return null;
            }
        }

        public async Task<IEnumerable<Farm>> GetFarmsAsync(string query)
        {
            FeedIterator<Farm> queryData = _container.GetItemQueryIterator<Farm>(new QueryDefinition(query));
            List<Farm> results = new List<Farm>();
            while (queryData.HasMoreResults)
            {
                FeedResponse<Farm> response = await queryData.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateFarmAsync(string id, Farm farm)
        {
            await _container.UpsertItemAsync<Farm>(farm, new PartitionKey(id));
        }
    }
}