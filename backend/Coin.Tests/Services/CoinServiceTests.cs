using Moq;
using Coin.Contracts.Persistence;
using Coin.Contracts.Repo;
using Coin.Contracts.Services;
using Coin.Data;
using Coin.DTO;
using Coin.Logic;

namespace Coin.Tests.Services
{
    /// <summary>
    /// Тесты для сервиса CoinService
    /// Проверяют корректность работы бизнес-логики операций с монетами
    /// </summary>
    public class CoinServiceTests
    {
        private readonly Mock<ICoinRepository> _mockCoinRepository;
        private readonly Mock<ICoinExchangeRepository> _mockCoinExchangeRepository;
        private readonly Mock<ICoinAPI> _mockCoinAPI;
        private readonly ICoinService _coinService;

        /// <summary>
        /// Конструктор - инициализация моков и тестируемого сервиса
        /// </summary>
        public CoinServiceTests()
        {
            _mockCoinRepository = new Mock<ICoinRepository>();
            _mockCoinExchangeRepository = new Mock<ICoinExchangeRepository>();
            _mockCoinAPI = new Mock<ICoinAPI>();
            _coinService = new CoinService(_mockCoinRepository.Object, _mockCoinExchangeRepository.Object, _mockCoinAPI.Object);
        }

        /// <summary>
        /// Тест: GetCoinsAllPreviousInformationAsync должен возвращать список монет
        /// </summary>
        [Fact]
        public async Task GetCoinsAllPreviousInformationAsync_ShouldReturnCoins()
        {
            // Arrange - подготовка тестовых данных
            var expectedCoins = new List<CoinDto>
            {
                new CoinDto { Id = 1, Name = "Bitcoin", Prices = 50000, VolumeTraded = 1000000 },
                new CoinDto { Id = 2, Name = "Ethereum", Prices = 3000, VolumeTraded = 500000 }
            };

            _mockCoinRepository
                .Setup(repo => repo.GetCoinsAllWithPreviousInformationAsync())
                .ReturnsAsync(expectedCoins);

            // Act - выполнение тестируемого метода
            var result = await _coinService.GetCoinsAllPreviousInformationAsync();

            // Assert - проверка результатов
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Bitcoin", result.First().Name);
            _mockCoinRepository.Verify(repo => repo.GetCoinsAllWithPreviousInformationAsync(), Times.Once);
        }

        /// <summary>
        /// Тест: GetCoinsAllPreviousInformationAsync должен возвращать пустой список, если монет нет
        /// </summary>
        [Fact]
        public async Task GetCoinsAllPreviousInformationAsync_ShouldReturnEmptyList_WhenNoCoins()
        {
            // Arrange
            var emptyList = new List<CoinDto>();
            _mockCoinRepository
                .Setup(repo => repo.GetCoinsAllWithPreviousInformationAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _coinService.GetCoinsAllPreviousInformationAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Тест: GetCoinRateAllByIdAsync должен возвращать курсы монеты
        /// </summary>
        [Fact]
        public async Task GetCoinRateAllByIdAsync_ShouldReturnCoinRates()
        {
            // Arrange
            int coinId = 1;
            int step = 24;
            var expectedRates = new List<CoinRateDto>
            {
                new CoinRateDto { Id = 1, CoinId = coinId, Prices = 50000, VolumeTraded = 1000000, Time = DateTime.Now },
                new CoinRateDto { Id = 2, CoinId = coinId, Prices = 51000, VolumeTraded = 1100000, Time = DateTime.Now.AddHours(24) }
            };

            _mockCoinExchangeRepository
                .Setup(repo => repo.GetCoinRateAllByIdAsync(coinId, step))
                .ReturnsAsync(expectedRates);

            // Act
            var result = await _coinService.GetCoinRateAllByIdAsync(coinId, step);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, rate => Assert.Equal(coinId, rate.CoinId));
            _mockCoinExchangeRepository.Verify(repo => repo.GetCoinRateAllByIdAsync(coinId, step), Times.Once);
        }

        /// <summary>
        /// Тест: GetCoinRateAllByIdAsync должен возвращать пустой список для несуществующей монеты
        /// </summary>
        [Fact]
        public async Task GetCoinRateAllByIdAsync_ShouldReturnEmptyList_WhenCoinNotFound()
        {
            // Arrange
            int coinId = 999;
            int step = 24;
            var emptyList = new List<CoinRateDto>();

            _mockCoinExchangeRepository
                .Setup(repo => repo.GetCoinRateAllByIdAsync(coinId, step))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _coinService.GetCoinRateAllByIdAsync(coinId, step);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Тест: DeleteCoinAsync должен вызывать метод удаления в репозитории
        /// </summary>
        [Fact]
        public async Task DeleteCoinAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            int coinId = 1;
            _mockCoinRepository
                .Setup(repo => repo.DeleteAsync(coinId))
                .Returns(Task.CompletedTask);

            // Act
            await _coinService.DeleteCoinAsync(coinId);

            // Assert
            _mockCoinRepository.Verify(repo => repo.DeleteAsync(coinId), Times.Once);
        }
    }
}

