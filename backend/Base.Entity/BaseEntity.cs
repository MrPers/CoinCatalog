using System.ComponentModel.DataAnnotations;

namespace Base.Entity
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
