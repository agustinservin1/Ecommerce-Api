namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> ExecuteTransactionAsync(Func<Task> action);
    }
}
