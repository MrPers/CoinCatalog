using Base.DTO;
using Letter.Contracts.Business;
using Letter.Contracts.Persistence;
using Letter.Contracts.Repo;
using Letter.Contracts.Services;
using Letter.Data;
using Letter.Data.Persistence;
using Letter.Data.Repository;
using Letter.Logic.Business;
using Letter.Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace Letter.Api
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
            services.Configure<MySettingsModelDto>(configuration.GetSection("MailConnection"));

            services.AddScoped(typeof(ILetterRepository), typeof(LetterRepository));
            services.AddScoped(typeof(ILetterBusiness), typeof(LetterBusiness));
            services.AddScoped<ILetterService, LetterService>();
            services.AddScoped(typeof(ILetterAPI), typeof(LetterAPI));
            services.AddAutoMapper(typeof(Mapper));

            // add services CORS
            services.AddCors();

            string connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection)); //SQL
            //services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(connection)); //Memory
            services.AddMemoryCache();  //System.AggregateException: 'Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: MailGraphAnalysis.Contracts.Services.ILetterService Lifetime: Scoped ImplementationType: 
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

            // add services CORS
            app.UseCors(builder => builder
                .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

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