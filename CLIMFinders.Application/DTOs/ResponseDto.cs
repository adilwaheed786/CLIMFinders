namespace CLIMFinders.Application.DTOs
{
    public class ResponseDto 
    {
        public int Id { get; set; }
        public int BusinessID { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Status { get; set; }
    }
}
