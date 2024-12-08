using Domain.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExecuteTransactionAsync(Func<Task> action)
        {
            //Using asegura que la transacción se cierre correctamente, liberando los recursos.

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action(); //ejecuta función pasada por parametro
                await transaction.CommitAsync(); //confirma los cambios de forma permanente
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(); //revierte cambios si hubo algún error
                return false;
            }
        }
    }

}
