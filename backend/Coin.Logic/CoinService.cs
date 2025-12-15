using Coin.Contracts.Persistence;
using Coin.Contracts.Repo;
using Coin.Contracts.Services;
using Coin.Data;
using Coin.DTO;
using Microsoft.Extensions.Logging;

namespace Coin.Logic
{
    public class CoinService : ICoinService
    {
        private readonly ICoinApiClient _coinApiClient;
        private readonly ICoinRepository _coinRepository;
        private readonly ICoinExchangeRepository _coinExchangeRepository;
        private readonly ILogger<CoinService> _logger;

        public CoinService(
            ICoinRepository coinRepository,
            ICoinExchangeRepository coinExchangeRepository,
            ICoinApiClient coinApiClient,
            ILogger<CoinService> logger)
        {
            _coinRepository = coinRepository;
            _coinExchangeRepository = coinExchangeRepository;
            _coinApiClient = coinApiClient;
            _logger = logger;
        }

        public async Task AddCoinHistoryAsync(string coinName, long startTicks = 0)
        {
            if (string.IsNullOrWhiteSpace(coinName))
                throw new ArgumentNullException(nameof(coinName));

            // Логика даты: если ticks переданы, берем их, иначе - год назад.
            var startDate = startTicks > 0
                ? DateTimeOffset.FromUnixTimeMilliseconds(startTicks).DateTime
                : DateTime.UtcNow.AddYears(-1);

            try
            {
                // 1. Получаем инфо о монете
                var coinDetails = await _coinApiClient.GetCoinDetailsAsync(coinName);
                if (coinDetails == null)
                    throw new InvalidOperationException($"Coin '{coinName}' not found via API.");

                // 2. Получаем исторические данные
                var history = await _coinApiClient.GetCoinHistoryAsync(coinDetails.Name, startDate);

                if (!history.Any())
                {
                    _logger.LogWarning("No history found for coin {CoinName} since {Date}", coinName, startDate);
                    return;
                }

                // 3. Сохраняем монету в БД и получаем её ID
                var storedCoinId = await _coinRepository.AddAsync(coinDetails);

                // 4. Привязываем историю к ID монеты
                foreach (var item in history)
                {
                    item.CoinId = storedCoinId;
                }

                // 5. Сохраняем историю пакетом
                await _coinExchangeRepository.AddCollectionAsync(history);

                _logger.LogInformation("Successfully added {Count} records for {CoinName}", history.Count, coinName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding coin history for {CoinName}", coinName);
                // Пробрасываем исключение дальше или оборачиваем в Custom Exception, но не теряем InnerException
                throw new InvalidOperationException($"Failed to process coin: {coinName}", ex);
            }
        }

        public async Task<ICollection<CoinDetailsDto>> GetCoinsAllPreviousInformationAsync()
        {
            return await _coinRepository.GetCoinsAllWithPreviousInformationAsync();
        }

        public async Task<ICollection<CoinPriceDto>> GetCoinRateAllByIdAsync(int id, int step)
        {
            // Ручная валидация, так как [Range] тут не сработает
            if (id < 1) throw new ArgumentOutOfRangeException(nameof(id));
            if (step < 24) throw new ArgumentOutOfRangeException(nameof(step), "Step must be at least 24 hours");

            return await _coinExchangeRepository.GetCoinsPricesByIdAsync(id, step);
        }

        public async Task<CoinDetailsDto> GetCoinsAllFullInformationAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException(nameof(id));

            var coin = await _coinRepository.GetCoinsAllFullInformationAsync(id);

            if (coin == null)
            {
                // Хорошая практика явно сообщать, если ничего не найдено, 
                // или возвращать null, чтобы контроллер вернул 404.
                _logger.LogWarning("Coin with id {Id} not found", id);
            }

            return coin;
        }

        public async Task UpdateCoinsByCoinIdAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException(nameof(id));

            try
            {
                var coin = await _coinRepository.GetCoinsAllFullInformationAsync(id);
                if (coin == null) throw new KeyNotFoundException($"Coin with ID {id} not found");

                var history = await _coinApiClient.GetCoinHistoryAsync(coin.Name, DateTime.UtcNow);

                if (history.Any())
                {
                    // Опять же: лучше делегировать присвоение ID репозиторию (как мы обсуждали),
                    // но если делаем тут, то через foreach (это быстрее и понятнее для отладки, чем .ToList().ForEach)
                    foreach (var item in history)
                    {
                        item.CoinId = id;
                    }

                    await _coinExchangeRepository.AddCollectionAsync(history);
                    _logger.LogInformation("Updated coin {Id}. Added {Count} records.", id, history.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update coin {Id}", id);
                throw; 
            }
        }

        public async Task DeleteCoinAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException(nameof(id));

            await _coinRepository.DeleteAsync(id);

            _logger.LogInformation("Coin {Id} deleted successfully", id);
        }
    }
}