using Moq;
using Letter.Contracts.Business;
using Letter.Contracts.Persistence;
using Letter.Contracts.Repo;
using Letter.Contracts.Services;
using Letter.DTO;
using Letter.Logic.Services;

namespace Letter.Tests.Services
{
    /// <summary>
    /// Тесты для сервиса LetterService
    /// Проверяют корректность работы бизнес-логики отправки email
    /// </summary>
    public class LetterServiceTests
    {
        private readonly Mock<ILetterRepository> _mockLetterRepository;
        private readonly Mock<ILetterAPI> _mockLetterAPI;
        private readonly Mock<ILetterBusiness> _mockLetterBusiness;
        private readonly ILetterService _letterService;

        /// <summary>
        /// Конструктор - инициализация моков и тестируемого сервиса
        /// </summary>
        public LetterServiceTests()
        {
            _mockLetterRepository = new Mock<ILetterRepository>();
            _mockLetterAPI = new Mock<ILetterAPI>();
            _mockLetterBusiness = new Mock<ILetterBusiness>();
            _letterService = new LetterService(
                _mockLetterRepository.Object,
                _mockLetterAPI.Object,
                _mockLetterBusiness.Object
            );
        }

        /// <summary>
        /// Тест: SendLetterAsync должен успешно отправлять письмо
        /// </summary>
        [Fact]
        public async Task SendLetterAsync_ShouldSendLetter_Successfully()
        {
            // Arrange
            var letterDto = new LetterDto
            {
                IdCoin = 1,
                StepCoin = 24,
                UserEmail = "test@example.com",
                TextSubject = "Test Subject",
                TextBody = "Test Body"
            };

            var coinRates = new List<Letter.DTO.CoinRateDto>
            {
                new Letter.DTO.CoinRateDto { Prices = 50000, VolumeTraded = 1000000, Time = DateTime.Now }
            };

            string expectedFilePath = @"C:\test.csv";

            _mockLetterAPI
                .Setup(api => api.GetCoinsRateDtoById(letterDto.IdCoin, letterDto.StepCoin))
                .ReturnsAsync(coinRates);

            _mockLetterBusiness
                .Setup(business => business.MakingFile(coinRates))
                .Returns(expectedFilePath);

            _mockLetterAPI
                .Setup(api => api.SendLetterAsync(It.IsAny<LetterDto>(), expectedFilePath))
                .Returns(Task.CompletedTask);

            _mockLetterRepository
                .Setup(repo => repo.AddAsync(It.IsAny<LetterDto>()))
                .ReturnsAsync(1);

            // Act
            await _letterService.SendLetterAsync(letterDto);

            // Assert
            _mockLetterAPI.Verify(api => api.GetCoinsRateDtoById(letterDto.IdCoin, letterDto.StepCoin), Times.Once);
            _mockLetterBusiness.Verify(business => business.MakingFile(coinRates), Times.Once);
            _mockLetterAPI.Verify(api => api.SendLetterAsync(It.IsAny<LetterDto>(), expectedFilePath), Times.Once);
            _mockLetterRepository.Verify(repo => repo.AddAsync(It.IsAny<LetterDto>()), Times.Once);
        }

        /// <summary>
        /// Тест: SendLetterAsync должен выбрасывать исключение при null параметре
        /// </summary>
        [Fact]
        public async Task SendLetterAsync_ShouldThrowException_WhenLetterIsNull()
        {
            // Arrange
            LetterDto? nullLetter = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _letterService.SendLetterAsync(nullLetter!));
        }

        /// <summary>
        /// Тест: SendLetterAsync должен обрабатывать ошибки API
        /// </summary>
        [Fact]
        public async Task SendLetterAsync_ShouldHandleAPIError()
        {
            // Arrange
            var letterDto = new LetterDto
            {
                IdCoin = 1,
                StepCoin = 24,
                UserEmail = "test@example.com",
                TextSubject = "Test Subject",
                TextBody = "Test Body"
            };

            _mockLetterAPI
                .Setup(api => api.GetCoinsRateDtoById(letterDto.IdCoin, letterDto.StepCoin))
                .ThrowsAsync(new Exception("API Error"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _letterService.SendLetterAsync(letterDto));
        }

        /// <summary>
        /// Тест: SendLetterAsync должен сохранять письмо в репозиторий
        /// </summary>
        [Fact]
        public async Task SendLetterAsync_ShouldSaveLetterToRepository()
        {
            // Arrange
            var letterDto = new LetterDto
            {
                IdCoin = 1,
                StepCoin = 24,
                UserEmail = "test@example.com",
                TextSubject = "Test Subject",
                TextBody = "Test Body"
            };

            var coinRates = new List<Letter.DTO.CoinRateDto>
            {
                new Letter.DTO.CoinRateDto { Prices = 50000, VolumeTraded = 1000000, Time = DateTime.Now }
            };

            _mockLetterAPI
                .Setup(api => api.GetCoinsRateDtoById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(coinRates);

            _mockLetterBusiness
                .Setup(business => business.MakingFile(It.IsAny<List<Letter.DTO.CoinRateDto>>()))
                .Returns(@"C:\test.csv");

            _mockLetterAPI
                .Setup(api => api.SendLetterAsync(It.IsAny<LetterDto>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _mockLetterRepository
                .Setup(repo => repo.AddAsync(It.IsAny<LetterDto>()))
                .ReturnsAsync(1);

            // Act
            await _letterService.SendLetterAsync(letterDto);

            // Assert
            _mockLetterRepository.Verify(
                repo => repo.AddAsync(It.Is<LetterDto>(l =>
                    l.UserEmail == letterDto.UserEmail &&
                    l.TextSubject == letterDto.TextSubject &&
                    l.TextBody == letterDto.TextBody
                )),
                Times.Once
            );
        }
    }
}

