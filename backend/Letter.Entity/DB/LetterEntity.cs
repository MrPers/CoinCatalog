using Base.Entity;

namespace Letter.Entity.DB
{
    public class LetterEntity : BaseEntity<int>
    {
        public DateTime TimeSend { get; set; }
        public string TextSubject { get; set; }
        public string TextBody { get; set; }
        public string UserEmail { get; set; }
    }
}
