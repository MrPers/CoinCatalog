using AutoMapper;
using Base.DTO;
using Letter.Contracts.Persistence;
using Letter.DTO;
using Letter.Entity;
using Letter.Entity.JSON;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using System;

namespace Letter.Data.Persistence
{
    public class LetterAPI : ILetterAPI
    {
        private IOptions<MySettingsModelDto> _appSettings;
        private readonly IMapper _mapper;
        private static string _url = "https://localhost:1000/Coin/get-coinExchanges";

        public LetterAPI(
            IOptions<MySettingsModelDto> appSettings, IMapper mapper
        )
        {
            _mapper = mapper;
            _appSettings = appSettings;
        }

        public async Task<List<CoinRateDto>> GetCoinsRateDtoById(int IdCoin, int StepCoin)
        {
            string result = await TakeCoinsFromResponseContentAsync(IdCoin, StepCoin);
            var rootObjects = JsonConvert.DeserializeObject<List<CoinRateJSON>>(result);

            if (rootObjects.Count() == 0)
            {
                throw new ArgumentException("The number of trades for this period is 0");
            }
            
            return _mapper.Map<List<CoinRateDto>>(rootObjects); ;
        }

        public async Task SendLetterAsync(LetterDto letter, string filePath)
        {
            MimeMessage emailMessage = CreateLetter(letter.TextBody, letter.TextSubject, letter.UserEmail, filePath);

            await SendLetterAsync(emailMessage);
        }

        private MimeMessage CreateLetter(string textBody, string textSubject, string usersEmail, string filePath)
        {
            var emailMessage = new MimeMessage();
            var builder = new BodyBuilder();

            emailMessage.From.Add(new MailboxAddress(_appSettings.Value.Name, _appSettings.Value.Address));
            emailMessage.To.Add(new MailboxAddress("", usersEmail));
            emailMessage.Subject = textSubject;
            builder.TextBody = textBody;
            builder.Attachments.Add(filePath);
            emailMessage.Body = builder.ToMessageBody();

            return emailMessage;
        }

        private async Task SendLetterAsync(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_appSettings.Value.SmtpClient, _appSettings.Value.Port, false);
                await client.AuthenticateAsync(_appSettings.Value.Address, _appSettings.Value.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);

                client.Dispose();
            }
        }
        
        private static async Task<string> TakeCoinsFromResponseContentAsync(int idCoin, int stepCoin)
        {
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(new CoinRateQuestion(idCoin, stepCoin));
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json"));
            HttpResponseMessage response = await client.PostAsync(_url, httpContent);

            client.Dispose();

            if ((int)response.StatusCode != 200)
            {
                throw new ArgumentException(await response.Content.ReadAsStringAsync());
            }

            return await response.Content.ReadAsStringAsync();
        }

    }
}
