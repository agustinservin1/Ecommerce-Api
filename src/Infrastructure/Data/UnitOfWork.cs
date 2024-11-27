using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            //Using asegura que la transacción se cierre correctamente, liberando los recursos.

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action(); //ejecuta función pasada por parametro
                await transaction.CommitAsync(); //confirma los cambios de forma permanente
            }
            catch
            {
                await transaction.RollbackAsync(); //revierte cambios si hubo algún error
                throw;// relanza la excepción 

            }
        }
    }

}
