using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Application.DTOs
{ 
    public class SubscriptionPlansDto
    {
        public int Id { get; set; }
        public string Tier { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; } 
        public List<PlanServicesDto> PlanServicesDto { get; set; }
    }
    public class PlanServicesDto
    { 
        public int Id { get; set; }
        public int PlanId { get; set; }
        public string Name { get; set; } 
    }
}
