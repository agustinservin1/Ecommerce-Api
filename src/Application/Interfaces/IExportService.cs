using System;
using System.Linq.Expressions;
using Application.Models.Export;

namespace Application.Interfaces
{
    public interface IExportService<T> where T : class
    {
        Expression<Func<T, bool>> BuildFilterExpression(ExportPropierty propertyName,
                                                        ExportOperation operation,
                                                        string value, string? value2 = null);
        Task<byte[]> ExportDataToExcel(Expression<Func<T, bool>> filterExpression);
    }
}

