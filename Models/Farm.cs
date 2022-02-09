using Newtonsoft.Json;

namespace AgricFameAPICosmosDB.Models
{
    public class Farm
    {

        //Azure Cosmos uses JSON to store Data
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "cropName")]
        public string CropName { get; set; }

        [JsonProperty(PropertyName = "cropDescription")]
        public string CropDescription { get; set; }
    }
}