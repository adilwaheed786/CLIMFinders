using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public class VehicleModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } 
        public int MakeId { get; set; }
        [ForeignKey("MakeId")]
        public VehicleMake VehicleMake { get; set; }
    }
}
