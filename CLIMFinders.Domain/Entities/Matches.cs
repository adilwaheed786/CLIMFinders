using CLIMFinders.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public class Matches: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public DateTime MatchedAt { get; set; }
        public bool Notified { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("VehicleId")] 
        public Vehicles Vehicles { get; set; }

    }
}
