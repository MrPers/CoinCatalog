using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Letter.Data
{
    public class DataSample
    {

        public static async Task InitializeAsync(IServiceScope serviceScope)
        {

            IServiceProvider scopeServiceProvider = serviceScope.ServiceProvider;

            var context = scopeServiceProvider.GetRequiredService<DataContext>();

            if (context.Database.EnsureCreated())
            {
                await scopeServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync();
            }


            //StringBuilder sb = new StringBuilder();
            //var comm = await context.Coins.ToListAsync();

            //using (var parser = new ChoJSONWriter(sb))
            //        parser.Write(comm);

            //Console.WriteLine(sb.ToString());

        }
    }

}
