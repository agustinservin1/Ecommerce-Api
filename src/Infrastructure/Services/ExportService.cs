using Application.Interfaces;
using ClosedXML.Excel;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExportService<T> : IExportService<T> where T : class
    {
        private readonly IBaseRepository<T> _repository;

        public ExportService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> ExportDataToExcel(Expression<Func<T, bool>> filter)
        {
            // datos filtrados de repo generico 
            var data = await _repository.Search(filter);

            using var workbook = new XLWorkbook();//aseguramos que se cierre el libro al final del bloque
            var worksheet = workbook.Worksheets.Add("Filtered Data");

            if (data != null && data.Any())
            {
                worksheet.Cell(1, 1).InsertTable(data);
            }

            using var stream = new MemoryStream();
            //flujo que nos permite leer y escribir datos dinamicos en memoria, a dif de filestream trabaja en ram en lugar de disco
            //si no se usa orm se puede implementar con DataTable y SQLDataAdapter + conexion manual
            workbook.SaveAs(stream);

            return stream.ToArray(); //devolvemos arreglo de bytes para 
        }
    }
}

