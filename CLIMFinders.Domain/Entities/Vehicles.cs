using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public class Vehicles: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string VIN { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int Year { get; set; } 
        public int ColorId { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public DateTime PickedOn { get;set;} = DateTime.Now;
        public int UserId { get; set; } 
        [ForeignKey("UserId")]
        public User Users { get; set; }
        [ForeignKey("MakeId")]
        public VehicleMake VehicleMake { get; set; } 
        [ForeignKey("ModelId")]
        public VehicleModel VehicleModel { get; set; }
        [ForeignKey("ColorId")]
        public VehicleColor VehicleColor { get; set; }
    }
}
