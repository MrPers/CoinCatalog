using AutoMapper;
using Coin.Api.Models;
using Coin.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Coin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase // Называем контроллер во множественном числе
    {
        private readonly ICoinService _coinService;
        private readonly IMapper _mapper;

        public CoinsController(ICoinService coinService, IMapper mapper)
        {
            _coinService = coinService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список всех монет (краткая информация)
        /// GET: api/coins
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<CoinDetailsVM>), 200)]
        public async Task<ActionResult<List<CoinDetailsVM>>> GetAll()
        {
            var coins = await _coinService.GetCoinsAllPreviousInformationAsync();
            var result = _mapper.Map<List<CoinDetailsVM>>(coins);
            return Ok(result);
        }

        /// <summary>
        /// Получить полную информацию о монете по ID
        /// GET: api/coins/5
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CoinFullDetailsVM), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CoinFullDetailsVM>> GetById([Range(1, int.MaxValue)] int id)
        {
            var coin = await _coinService.GetCoinsAllFullInformationAsync(id);

            if (coin == null) return NotFound($"Coin with ID {id} not found");

            var result = _mapper.Map<CoinFullDetailsVM>(coin);
            return Ok(result);
        }

        /// <summary>
        /// Получить историю курсов монеты
        /// GET: api/coins/history?id=1&step=24&inTick=true
        /// </summary>
        /// <remarks>
        /// Мы используем [FromQuery], чтобы передать параметры в URL, а не в теле запроса.
        /// Это стандарт для получения данных.
        /// </remarks>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] CoinRateQuestion query)
        {
            // Валидацию лучше вынести в сам класс DTO через DataAnnotations, но оставим проверку здесь для наглядности
            if (query.Id < 1 || query.Step < 24)
            {
                return BadRequest("Invalid ID or Step (minimum step is 24 hours)");
            }

            var rates = await _coinService.GetCoinRateAllByIdAsync(query.Id, query.Step);

            // Логику выбора маппинга лучше держать простой
            if (query.InTick)
            {
                return Ok(_mapper.Map<List<CoinPriceVMInTicks>>(rates));
            }

            return Ok(_mapper.Map<List<CoinPriceVMInDateTime>>(rates));
        }

        /// <summary>
        /// Добавить новую монету и загрузить историю
        /// POST: api/coins?name=bitcoin&ticks=0
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201)] // 201 Created - стандарт для создания
        public async Task<IActionResult> Create([FromQuery] string name = "bitcoin", [FromQuery] long ticks = 0)
        {
            // В идеале параметры передавать в [FromBody] CreateCoinDto, но query тоже допустим для простых типов
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required");

            try
            {
                await _coinService.AddCoinHistoryAsync(name, ticks);
                // По REST мы должны вернуть ссылку на созданный ресурс, но пока вернем просто Ok
                return StatusCode(201, "Coin created and history loaded");
            }
            catch (ArgumentException ex) // Ловим специфичные ошибки логики
            {
                return BadRequest(ex.Message);
            }
            // Остальные ошибки (500) лучше ловить глобальным Middleware
        }

        /// <summary>
        /// Обновить данные монеты
        /// PUT: api/coins/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([Range(1, int.MaxValue)] int id)
        {
            await _coinService.UpdateCoinsByCoinIdAsync(id);
            return NoContent(); // 204 No Content - стандарт для обновления, когда не нужно возвращать данные
        }

        /// <summary>
        /// Удалить монету
        /// DELETE: api/coins/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Range(1, int.MaxValue)] int id)
        {
            await _coinService.DeleteCoinAsync(id);
            return NoContent(); // 204 No Content
        }
    }
}