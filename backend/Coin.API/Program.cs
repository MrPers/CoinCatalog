using Coin.Data;
using Coin.Contracts.Repo;
using Coin.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using Coin.Data.Repository;
using Coin.Data.Persistence;
using Coin.Contracts.Persistence;
using Coin.Logic;

namespace Coin.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;
            IServiceCollection services = builder.Services;
            ConfigureServices(services, configuration);

            var host = builder.Build();
            Configure(host, builder.Environment);
            await SeedDatabaseAsync(host);
            await host.RunAsync();
            
        }

        public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpClient(); // to send requests

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //register a class
            services.AddScoped(typeof(ICoinRepository), typeof(CoinRepository));
            services.AddScoped(typeof(ICoinExchangeRepository), typeof(CoinExchangeRepository));
            services.AddScoped(typeof(ICoinApiClient), typeof(CoinApiClient));
            services.AddHttpClient<ICoinApiClient, CoinApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
                client.DefaultRequestHeaders.Add("User-Agent", "MyCryptoApp/1.0");
            });
            services.AddScoped<ICoinService, CoinService>();
            services.AddAutoMapper(cfg => cfg.AddProfile<Mapper>());

            // add services CORS
            services.AddCors();

            string connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection)); //SQL
            //services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(connection)); //Memory
            services.AddMemoryCache();  
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // add services CORS
            app.UseCors(builder => builder
                .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();
            app.UseEndpoints(x => x.MapDefaultControllerRoute());

        }

        private static async Task SeedDatabaseAsync(WebApplication host)
        {

            using (var scope = host.Services.CreateScope())
            {
                await DataSample.InitializeAsync(scope);
            }
        }

    }
}