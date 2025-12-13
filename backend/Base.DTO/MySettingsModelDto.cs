using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DTO
{
    public class MySettingsModelDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }//
        public string SmtpClient { get; set; }
        public string TicksInHour { get; set; }
        public int Port { get; set; }
    }
}
