using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Сoin.Api.Controller;
using Сoin.Api.Models;
using Сoin.Contracts.Services;
using Сoin.DTO;

namespace Сoin.Tests.Controllers
{
    /// <summary>
    /// Тесты для контроллера CoinController
    /// Проверяют корректность обработки HTTP запросов
    /// </summary>
    public class CoinControllerTests
    {
        private readonly Mock<ICoinService> _mockCoinService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CoinController _controller;

        /// <summary>
        /// Конструктор - инициализация моков и тестируемого контроллера
        /// </summary>
        public CoinControllerTests()
        {
            _mockCoinService = new Mock<ICoinService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CoinController(_mockCoinService.Object, _mockMapper.Object);
        }

        /// <summary>
        /// Тест: GetCoinsAllPreviousInformation должен возвращать Ok с данными
        /// </summary>
        [Fact]
        public async Task GetCoinsAllPreviousInformation_ShouldReturnOk_WithCoins()
        {
            // Arrange
            var coinDtos = new List<CoinDto>
            {
                new CoinDto { Id = 1, Name = "Bitcoin", Prices = 50000 },
                new CoinDto { Id = 2, Name = "Ethereum", Prices = 3000 }
            };

            var coinVMs = new List<CoinVM>
            {
                new CoinVM { Id = 1, Name = "Bitcoin", Prices = 50000 },
                new CoinVM { Id = 2, Name = "Ethereum", Prices = 3000 }
            };

            _mockCoinService
                .Setup(service => service.GetCoinsAllPreviousInformationAsync())
                .ReturnsAsync(coinDtos);

            _mockMapper
                .Setup(mapper => mapper.Map<List<CoinVM>>(coinDtos))
                .Returns(coinVMs);

            // Act
            var result = await _controller.GetCoinsAllPreviousInformation();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCoins = Assert.IsType<List<CoinVM>>(okResult.Value);
            Assert.Equal(2, returnedCoins.Count);
        }

        /// <summary>
        /// Тест: GetCoinsAllPreviousInformation должен возвращать BadRequest при ошибке
        /// </summary>
        [Fact]
        public async Task GetCoinsAllPreviousInformation_ShouldReturnBadRequest_OnException()
        {
            // Arrange
            _mockCoinService
                .Setup(service => service.GetCoinsAllPreviousInformationAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetCoinsAllPreviousInformation();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Database error", badRequestResult.Value);
        }

        /// <summary>
        /// Тест: GetCoinFullInformation должен возвращать Ok с данными монеты
        /// </summary>
        [Fact]
        public async Task GetCoinFullInformation_ShouldReturnOk_WithCoinData()
        {
            // Arrange
            int coinId = 1;
            var coinDto = new CoinDto { Id = coinId, Name = "Bitcoin", Prices = 50000 };
            var coinVM = new CoinFullVM { Id = coinId, Name = "Bitcoin" };

            _mockCoinService
                .Setup(service => service.GetCoinsAllFullInformationAsync(coinId))
                .ReturnsAsync(coinDto);

            _mockMapper
                .Setup(mapper => mapper.Map<CoinFullVM>(coinDto))
                .Returns(coinVM);

            // Act
            var result = await _controller.GetCoinFullInformation(coinId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCoin = Assert.IsType<CoinFullVM>(okResult.Value);
            Assert.Equal(coinId, returnedCoin.Id);
            Assert.Equal("Bitcoin", returnedCoin.Name);
        }

        /// <summary>
        /// Тест: DeleteCoinAndCoinRate должен возвращать Ok при успешном удалении
        /// </summary>
        [Fact]
        public async Task DeleteCoinAndCoinRate_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            int coinId = 1;
            _mockCoinService
                .Setup(service => service.DeleteCoinAsync(coinId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCoinAndCoinRate(coinId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value!);
            _mockCoinService.Verify(service => service.DeleteCoinAsync(coinId), Times.Once);
        }

        /// <summary>
        /// Тест: DeleteCoinAndCoinRate должен возвращать BadRequest при ошибке
        /// </summary>
        [Fact]
        public async Task DeleteCoinAndCoinRate_ShouldReturnBadRequest_OnException()
        {
            // Arrange
            int coinId = 1;
            _mockCoinService
                .Setup(service => service.DeleteCoinAsync(coinId))
                .ThrowsAsync(new Exception("Coin not found"));

            // Act
            var result = await _controller.DeleteCoinAndCoinRate(coinId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Coin not found", badRequestResult.Value);
        }
    }
}

