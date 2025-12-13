using Base.Entity;
using System.ComponentModel.DataAnnotations;

namespace Base.DTO
{
    public class BaseDto<T>// : IBaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
