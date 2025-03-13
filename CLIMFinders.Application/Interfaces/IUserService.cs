namespace CLIMFinders.Application.Interfaces
{
    public interface IUserService
    {
        int GetUserId();
        int GetBusinessId();
        string GeneratePassword(int length);
    }
}
