using AgricFameAPICosmosDB.Services;
using AgricFameAPICosmosDB.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace AgricFameAPICosmosDB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IFarmCosmosDbService>(AddAzureCosmosDbModule(Configuration.GetSection("AzureCosmosDb")).GetAwaiter().GetResult());
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgricFameAPICosmosDB", Version = "v1" });
            });

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgricFameAPICosmosDB v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static async Task<FarmCosmosDbService> AddAzureCosmosDbModule(IConfiguration configuration)
        {
            string databaseName = configuration.GetValue<string>("DatabaseName");
            string containerName = configuration.GetValue<string>("ContainerName");
            string account = configuration.GetValue<string>("Account");
            string key = configuration.GetValue<string>("Key");

            CosmosClient client = new CosmosClient(account, key);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            FarmCosmosDbService cosmosDbService = new FarmCosmosDbService(client, databaseName, containerName);

            return cosmosDbService;
        }


    }
}