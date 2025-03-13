 namespace CLIMFinders.Application.Interfaces
{
    public interface IUnitOfWork
    {

        IRepositoryBase<T> GetRepository<T>()
            where T : class;
        
    }
}
