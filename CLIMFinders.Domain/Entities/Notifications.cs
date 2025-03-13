using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public class Notifications: BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
