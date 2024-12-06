using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExportService<T> where T : class
    {
       Task<byte[]> ExportDataToExcel(Expression<Func<T, bool>> filter);
    }
}

