using Letter.Contracts.Business;
using Letter.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Letter.Logic.Business
{
    public class LetterBusiness : ILetterBusiness
    {
        public String MakingFile(List<CoinRateDto> coins)
        {
            string writeText = "Time, Prices, Volume Traded \r\n";
            foreach (var coin in coins)
            {
                writeText += coin.Time + "," + coin.Prices + "," + coin.VolumeTraded + "\r\n";
            }

            var time = (int)(DateTime.Now.Ticks % int.MaxValue);
            File.WriteAllText($@"C:\{time}.csv", writeText);  // Create a file and write the content of writeText to it
            //File.Delete(@"C:\text.csv");

            return $@"C:\{time}.csv";
        }
    }
}
