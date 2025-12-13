using AutoMapper;
using Сoin.Contracts.Repo;
using Сoin.Contracts.Services;
using Сoin.DTO;
using System.ComponentModel.DataAnnotations;
using Сoin.Contracts.Persistence;
using Сoin.Data;

namespace Сoin.Logic
{
    /// <summary>
    /// Сервис для работы с криптовалютами
    /// Реализует бизнес-логику операций с монетами и их курсами
    /// </summary>
    public class CoinService : ICoinService
    {
        private readonly ICoinAPI _сoinFromAPI;
        private readonly ICoinRepository _coinRepository;
        private readonly ICoinExchangeRepository _coinExchangeRepository;
        private const long data2020 = 1546300800000;
        private const long data2021 = 1577836800000;
        private const long data1970 = 621355968000000000;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="dispatchRepository">Репозиторий для работы с монетами</param>
        /// <param name="coinExchangeRepository">Репозиторий для работы с курсами</param>
        /// <param name="сoinFromAPI">API для получения данных о монетах</param>
        public CoinService(ICoinRepository dispatchRepository, ICoinExchangeRepository coinExchangeRepository, ICoinAPI сoinFromAPI)
        {
            _coinExchangeRepository = coinExchangeRepository;
            _coinRepository = dispatchRepository;
            _сoinFromAPI = сoinFromAPI;
        }

        /// <summary>
        /// Получение всех монет с предыдущей информацией
        /// </summary>
        /// <returns>Коллекция монет</returns>
        public async Task<ICollection<CoinDto>> GetCoinsAllPreviousInformationAsync()
        {
            var coins = await _coinRepository.GetCoinsAllWithPreviousInformationAsync();

            return coins;
        }

        /// <summary>
        /// Получение всех курсов монеты по ID с заданным шагом
        /// </summary>
        /// <param name="id">Идентификатор монеты</param>
        /// <param name="step">Шаг времени в часах</param>
        /// <returns>Коллекция курсов</returns>
        public async Task<ICollection<CoinRateDto>> GetCoinRateAllByIdAsync([Range(1, int.MaxValue)] int id, [Range(24, int.MaxValue)] int step)
        {
            var coins = await _coinExchangeRepository.GetCoinRateAllByIdAsync(id, step);

            return coins;
        }

        /// <summary>
        /// Получение полной информации о монете по ID
        /// </summary>
        /// <param name="id">Идентификатор монеты</param>
        /// <returns>Полная информация о монете</returns>
        public async Task<CoinDto> GetCoinsAllFullInformationAsync([Range(1, int.MaxValue)] int id)
        {
            var coin = await _coinRepository.GetCoinsAllFullInformationAsync(id);

            return coin;
        }

        /// <summary>
        /// Добавление новой монеты с историей курсов
        /// </summary>
        /// <param name="name">Название монеты</param>
        /// <param name="ticks">Временная метка начала отслеживания</param>
        public async Task AddСoinСoinExchangesAsync(string name, [Range(data2020, long.MaxValue)] long ticks = data2021)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter cannot be null", nameof(name));
            }

            try
            {
                var data = new DateTime().AddTicks(data1970 + ticks * 10000);
                var newCoins = await _сoinFromAPI.TakeCoinNameFromAPIAsync(name);
                var coinExchanges = await _сoinFromAPI.TakeCoinsFromAPIAsync(newCoins.Name, data);
                var answerCoins = await _coinRepository.AddAsync(newCoins);
                coinExchanges.ToList().ForEach(x => x.CoinId = answerCoins);
                await _coinExchangeRepository.AddCollectionAsync(coinExchanges);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Coin not found {name}", ex.Message);
            }
        }

        /// <summary>
        /// Обновление данных монеты по ID
        /// Получает новые курсы с момента последнего обновления
        /// </summary>
        /// <param name="id">Идентификатор монеты</param>
        public async Task UpdateCoinsByCoinIdAsync([Range(1, int.MaxValue)] int id)
        {
            try
            {
                var lastDateTime = await _coinExchangeRepository.GetLastCoinRepositoryAsync(id);
                if (DateTime.Now.Ticks - lastDateTime.AddHours(8).Ticks < 0)
                {
                    throw new ArgumentException("8 hours have not yet passed for the update");
                }
                var coin = await _coinRepository.GetCoinsAllFullInformationAsync(id);
                var coinExchanges = await _сoinFromAPI.TakeCoinsFromAPIAsync(coin.Name, lastDateTime.AddHours(8));
                coinExchanges.ToList().ForEach(x => x.CoinId = id);
                await _coinExchangeRepository.AddCollectionAsync(coinExchanges);
            }
            catch (Exception er)
            {
                throw new ArgumentException(er.Message);
            }
        }

        /// <summary>
        /// Удаление монеты и всех связанных курсов
        /// </summary>
        /// <param name="id">Идентификатор монеты</param>
        public async Task DeleteCoinAsync([Range(1, int.MaxValue)] int id)
        {
            try
            {
                await _coinRepository.DeleteAsync(id);
            }
            catch (Exception er)
            {
                throw new ArgumentException(er.Message);
            }
        }

    }
}
