using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public class Subscriptions: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }         
        public int TierId { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; }
        [ForeignKey("TierId")]
        public SubscriptionPlans SubscriptionPlans { get; set; }
    }
}
