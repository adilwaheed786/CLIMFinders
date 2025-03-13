using System.ComponentModel.DataAnnotations.Schema;

namespace CLIMFinders.Domain.Entities
{
    public class Payments: BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
