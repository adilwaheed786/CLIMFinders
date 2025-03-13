using System.ComponentModel.DataAnnotations;

namespace CLIMFinders.Domain.Entities
{
    public partial class Roles 
    {
        [Key]
        public int Id { get; set; }
        public string RoleNanme { get; set; }   
    }
    public partial class SubRoles 
    {
        [Key]
        public int Id { get; set; }
        public string RoleNanme { get; set; }
    }
}
