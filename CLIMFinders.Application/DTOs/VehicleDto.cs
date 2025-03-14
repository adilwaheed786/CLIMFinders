using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Application.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN must be exactly 17 characters.")]
        [RegularExpression(@"^[A-HJ-NPR-Z0-9]+$", ErrorMessage = "VIN can only contain letters (A-Z, except I, O, Q) and numbers (0-9).")]

        [DisplayName("Vehicle Identification Number")]
        public string VIN { get; set; }
        [DisplayName("Make")]
        public int MakeId { get; set; }
        [DisplayName("Model")]
        public int ModelId { get; set; }
        [DisplayName("Year")]
        public int Year { get; set; }
        [DisplayName("Color")]
        public int ColorId { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        [DisplayName("Impoundent Location Details")]
        public string LocationDetails { get; set; }
        [DisplayName("Contact Information")]
        public string ContactInformation { get; set; }
        [DisplayName("Impound Fees /Towing charges")]
        public int ImpoundFees { get; set; }
        [DisplayName("Reason for Impoundment")]
        public string ReasonImpoundent { get; set; }
        [DisplayName("Vehicle Pickup Date & Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime PickedOn { get; set; } = DateTime.Now;
        public int BusinessId { get; set; }
    }
    public class VehicleTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SearchResultDto
    {
        public List<VehicleListDto> Result { get; set; } = [];
        public string Status { get; set; } = string.Empty;
    }

    public class VehicleListDto
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public int MakeId { get; set; }
        public string Make { get; set; }
        public int ModelId { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int ColorId { get; set; }
        public string Color { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public string? CompanyName { get; set; }
        public string BoundStatus { get; set; }
        public string ContactInformation { get; set; }
        public string ImpoundFees { get; set; }
        public string LocationDetails { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]

        public DateTime PickedOn { get; set; }
    }

}
