namespace CLIMFinders.Application.DTOs
{
    public class GenericResponse(int Id, int UserId, object? ReturnMessage, object? ReturnData, bool ReturnStatus)
    {
        public int Id { get; set; } = Id;
        public int UserId { get; set; } = UserId;
        public object? ReturnMessage { get; set; } = ReturnMessage;
        public object? ReturnData { get; set; } = ReturnData;
        public bool ReturnStatus { get; set; } = ReturnStatus;
    }
}
