using AutoMapper;
using Letter.Contracts.Services;
using Letter.DTO;
using Letter.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Letter.Api.Controller
{
    /// <summary>
    /// Контроллер для работы с email уведомлениями
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LetterController : ControllerBase
    {
        private readonly ILetterService _letterService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="letterService">Сервис для работы с письмами</param>
        /// <param name="mapper">Маппер для преобразования объектов</param>
        public LetterController(
            ILetterService letterService,
            IMapper mapper
        )
        {
            _letterService = letterService;
            _mapper = mapper;
        }

        /// <summary>
        /// Отправка email сообщения
        /// </summary>
        /// <param name="letter">Данные письма (адрес получателя, тема, текст)</param>
        /// <returns>Результат операции</returns>
        [HttpPost("send-email-addresses")]
        public async Task<IActionResult> SendLetter(LetterVM letter)
        {
            if (letter == null || letter.UserEmail == null || letter.TextSubject == null || letter.TextBody == null)
            {
                throw new ArgumentNullException(nameof(letter));
            }

            try
            {
                var lettersDto = _mapper.Map<LetterDto>(letter);

                await _letterService.SendLetterAsync(lettersDto);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}