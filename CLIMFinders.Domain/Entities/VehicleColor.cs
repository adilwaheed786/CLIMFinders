using System.ComponentModel.DataAnnotations;

namespace CLIMFinders.Domain.Entities
{
    public class VehicleColor
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
