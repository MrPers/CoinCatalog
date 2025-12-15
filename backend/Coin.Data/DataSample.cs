using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coin.Data
{
    public class DataSample
    {
        public static async Task InitializeAsync(IServiceScope serviceScope)
        {
            IServiceProvider scopeServiceProvider = serviceScope.ServiceProvider;

            var context = scopeServiceProvider.GetRequiredService<DataContext>();

            // Применяем миграции для создания или обновления базы данных
            await context.Database.MigrateAsync();

            // Закомментировано из-за ограничений API
            // var _coinService = scopeServiceProvider.GetRequiredService<ICoinService>();

            // if (!context.Coins.Any() & !context.CoinRates.Any())
            // {
            //     var coinsName = new List<String>
            //     {
            //         "bitcoin",
            //         "ethereum"
            //     };

            //     foreach (var name in coinsName)
            //     {
            //         await _coinService.AddCoinCoinExchangesAsync(name);
            //     }
            // }

        }
    }

}
