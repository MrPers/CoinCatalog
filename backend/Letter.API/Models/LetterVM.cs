using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Letter.Api.Models
{
    public class LetterVM
    {
        public string TextSubject { get; set; }
        public string TextBody { get; set; }
        public string UserEmail { get; set; }
        public int IdCoin { get; set; }
        public int StepCoin { get; set; }
    }
}
