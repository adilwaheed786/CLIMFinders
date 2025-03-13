using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public partial class PlanServices
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public int PlanId { get; set; }
        [ForeignKey("PlanId")] 
        public SubscriptionPlans? SubscriptionPlans { get; set; }      
    } 
}