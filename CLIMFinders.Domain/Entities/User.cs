using CLIMFinders.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public partial class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }  
        public string Email { get; set; }
       // public string Password { get; set; }
        public int RoleId { get; set; }
        public int? SubRoleId { get; set; }
        public int? TierId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool? IsConfirmed { get; set; } = false;
        public DateTime? ConfirmedOn { get; set; }         
        public byte[]? Photo { get; set; }
        [ForeignKey("RoleId")]
        public Roles Roles { get; set; }
        [ForeignKey("SubRoleId")]
        public SubRoles? SubRoles { get; set; } 
        public UserAddress? Businesses { get; set; }         
        public string ConfirmationCode { get; set; } = string.Empty;
        public string SubscriptionId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        [ForeignKey("TierId")]
        public SubscriptionPlans SubscriptionPlans { get; set; }
    }
}
