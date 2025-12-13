using Letter.Contracts.Business;
using Letter.Contracts.Persistence;
using Letter.Contracts.Repo;
using Letter.Contracts.Services;
using Letter.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace Letter.Logic.Services
{
    /// <summary>
    /// Сервис для работы с email уведомлениями
    /// Реализует бизнес-логику отправки писем с данными о криптовалютах
    /// </summary>
    public class LetterService : ILetterService
    {
        private ILetterRepository _letterRepository;
        private ILetterAPI _letterAPI;
        private ILetterBusiness _letterBusiness;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="dispatchRepository">Репозиторий для работы с письмами</param>
        /// <param name="letterAPI">API для отправки писем</param>
        /// <param name="letterBusiness">Бизнес-логика для формирования файлов</param>
        public LetterService(
            ILetterRepository dispatchRepository,
            ILetterAPI letterAPI,
            ILetterBusiness letterBusiness
        )
        {
            _letterRepository = dispatchRepository;
            _letterAPI = letterAPI;
            _letterBusiness = letterBusiness;
        }

        /// <summary>
        /// Отправка email с данными о криптовалюте
        /// Получает данные о монете, формирует файл и отправляет письмо
        /// </summary>
        /// <param name="letters">Данные письма</param>
        public async Task SendLetterAsync(LetterDto letters)
        {
            if (letters == null)
            {
                throw new ArgumentNullException(nameof(letters));
            }

            try
            {
                var coins = await _letterAPI.GetCoinsRateDtoById(letters.IdCoin, letters.StepCoin);

                string filePath = _letterBusiness.MakingFile(coins);

                await _letterAPI.SendLetterAsync(letters, filePath);

                await _letterRepository.AddAsync(new LetterDto()
                {
                    TimeSend = DateTime.Now,
                    TextBody = letters.TextBody,
                    TextSubject = letters.TextSubject,
                    UserEmail = letters.UserEmail
                });
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(nameof(letters));
            }
        }

    }
}