using System.ComponentModel.DataAnnotations;

namespace CLIMFinders.Domain.Entities
{
    public partial class SubscriptionPlans
    {
        [Key]
        public int Id { get; set; }
        public string Tier { get; set; }
        public decimal Amount { get; set; } 
        public int Duration { get; set; } 
        public PlanServices PlanServices { get; set; }

    }
}
