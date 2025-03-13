namespace CLIMFinders.Application.Interfaces
{
    public interface IEmailHelperUtils
    {
        string FillEmailContents(
            object dataToFill,
            string fileName,
            string Name
        );
    }
}
