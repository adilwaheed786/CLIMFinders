using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public class Searches: BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string VIN { get; set; }
        public DateTime SearchDate { get; set; }
        public bool Paid { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } 

    }
}
