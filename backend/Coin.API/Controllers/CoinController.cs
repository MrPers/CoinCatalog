using AutoMapper;
using Coin.Contracts.Services;
using Coin.Api.Models;
using Coin.Entity;
using Coin.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coin.Api.Controller
{
    /// <summary>
    /// Контроллер для работы с криптовалютами
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CoinController : ControllerBase
    {
        private readonly ICoinService _coinService;
        private readonly IMapper _mapper;
        private const long data2020 = 1546300800000;
        private const long data2021 = 1577836800000;
        private const string nameBTC = "bitcoin";

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="coinService">Сервис для работы с монетами</param>
        /// <param name="mapper">Маппер для преобразования объектов</param>
        public CoinController(
            ICoinService coinService,
            IMapper mapper
        )
        {
            _coinService = coinService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка всех монет с предыдущей информацией
        /// </summary>
        /// <returns>Список монет с информацией</returns>
        [HttpGet("get-coins-all-previous-information")]
        public async Task<IActionResult> GetCoinsAllPreviousInformation()
        {
            try
            {
                var coins = await _coinService.GetCoinsAllPreviousInformationAsync();
                var coinsResult = _mapper.Map<List<CoinVM>>(coins);

                return Ok(coinsResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение полной информации о монете по ID
        /// </summary>
        /// <param name="id">Идентификатор монеты</param>
        /// <returns>Полная информация о монете</returns>
        [HttpGet("get-coin-full-information-by-coin-id/{id}")]
        public async Task<IActionResult> GetCoinFullInformation([Range(1, int.MaxValue)] int id)
        {
            try
            {
                var coins = await _coinService.GetCoinsAllFullInformationAsync(id);
                var coinsResult = _mapper.Map<CoinFullVM>(coins);

                return Ok(coinsResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение курсов монеты за определенный период
        /// </summary>
        /// <param name="coinRateQuestion">Параметры запроса (ID монеты, шаг времени)</param>
        /// <returns>Список курсов монеты</returns>
        [HttpPost("get-coinExchanges")]
        public async Task<IActionResult> GetCoinsById(CoinRateQuestion coinRateQuestion)
        {
            if (coinRateQuestion == null || coinRateQuestion.Id < 1 || coinRateQuestion.Step < 24)
            {
                throw new ArgumentNullException(nameof(coinRateQuestion));
            }

            try
            {
                var coins = await _coinService.GetCoinRateAllByIdAsync(coinRateQuestion.Id, coinRateQuestion.Step);

                return Ok(coinRateQuestion.InTick ? _mapper.Map<List<CoinRateVMInTicks>>(coins) : _mapper.Map<List<CoinRateVMInDateTime>>(coins));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавление новой монеты с историей курсов
        /// </summary>
        /// <param name="name">Название монеты</param>
        /// <param name="ticks">Временная метка начала отслеживания</param>
        /// <returns>Результат операции</returns>
        [HttpPost("add-coin-&-coinExchanges/{name}")]
        public async Task<IActionResult> AddCoinCoinExchangesAsync([FromRoute] string name = nameBTC, [Range(data2020, long.MaxValue)] long ticks = data2021)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ValidationProblem();
            }

            try
            {
                await _coinService.AddCoinCoinExchangesAsync(name, ticks);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновление данных монеты
        /// </summary>
        /// <param name="id">Идентификатор монеты</param>
        /// <returns>Результат операции</returns>
        [HttpGet("update-coin-by-id-coin/{id}")]
        public async Task<IActionResult> UpdateCoinAsync([Range(1, int.MaxValue)] int id)
        {
            try
            {
                await _coinService.UpdateCoinsByCoinIdAsync(id);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удаление монеты и всех связанных курсов
        /// </summary>
        /// <param name="id">Идентификатор монеты</param>
        /// <returns>Результат операции</returns>
        [HttpDelete("delete-coin-and-coinExchanges/{id}")]
        public async Task<IActionResult> DeleteCoinAndCoinRate([Range(1, int.MaxValue)]int id)
        {
            try
            {
                await _coinService.DeleteCoinAsync(id);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
