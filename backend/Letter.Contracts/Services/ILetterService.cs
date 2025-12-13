using Letter.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Letter.Contracts.Services
{
    public interface ILetterService
    {
        Task SendLetterAsync(LetterDto letters);
    }
}
