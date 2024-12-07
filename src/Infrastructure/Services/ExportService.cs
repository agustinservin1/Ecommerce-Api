using Application.Interfaces;
using Application.Models.Export;
using ClosedXML.Excel;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public class ExportService<T> : IExportService<T> where T : class
    {
        private readonly IBaseRepository<T> _repository;

        public ExportService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }
        public Expression<Func<T, bool>> BuildFilterExpression(
            ExportPropierty propertyName,
            ExportOperation operation,
            string value, string? value2 = null)
        {

            Type propertyType = typeof(T).GetProperty(propertyName.ToString()).PropertyType;
            var convertedValue = ConvertValue(value, propertyType);
            object? convertedValue2 = value2 != null ? ConvertValue(value2, propertyType) : null;

            var parameter = Expression.Parameter(typeof(T), "x"); //creacion del parametro x representa instancia generica
            var property = Expression.Property(parameter, propertyName.ToString());//accede a la propiedad especificada con el nombre(del obj generico)
            var constant = Expression.Constant(convertedValue, propertyType); //constante con valor proporcionado para comparar con la propiedad 

            Expression comparison = operation switch //en base al enum que nos viene por query realizamos la op correspondiente
            {
                ExportOperation.Equals => Expression.Equal(property, constant),
                ExportOperation.Contains => BuildContainsExpression(property, constant), 
                ExportOperation.GreaterThan => Expression.GreaterThan(property, constant),
                ExportOperation.LessThan => Expression.LessThan(property, constant),
                ExportOperation.Between => BuildBetweenExpression(property, constant, convertedValue2),
                _=> throw new NotSupportedException($"Operation '{operation}' is not supported.")
            };

            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }
        public async Task<byte[]> ExportDataToExcel(Expression<Func<T, bool>> filterExpression)
        {
            var data = await _repository.Search(filterExpression); // datos filtrados desde el repo generico
            if (data == null || !data.Any()) // verifica que no se creen archivos vacíos
            {
                throw new InvalidOperationException("No se encontraron productos que coincidan con el filtro.");
            }
            using var workbook = new XLWorkbook(); //using asegura que se cierre el libro al final del bloque
            var worksheet = workbook.Worksheets.Add("Filtered Data");
            worksheet.Cell(1, 1).InsertTable(data);
            using var stream = new MemoryStream();  //flujo que nos permite leer y escribir datos dinamicos en memoria, a dif de filestream trabaja en ram en lugar de disco
            //si no se usa orm se puede implementar con DataTable y SQLDataAdapter + conexion manual
            workbook.SaveAs(stream);
            return stream.ToArray();
            }
        private static object ConvertValue(string value, Type targetType) //manejamos el string del query convirtiendo a dato especifico
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("El valor no puede ser nulo o vacío.", nameof(value));
            }
            //   si el tipo de destino no es decimal usa changetype de la clase convert
            return targetType == typeof(decimal) ? decimal.Parse(value) : Convert.ChangeType(value, targetType);
        }
        private static MethodCallExpression BuildContainsExpression(MemberExpression property, ConstantExpression constant) //llamamos al metodo contains de string
        {
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            return Expression.Call(property, containsMethod, constant);
        }


        private static Expression BuildBetweenExpression(MemberExpression property, ConstantExpression constant, object value2)
        {
            if (value2 == null) throw new ArgumentException("The 'between' operation requires two values.");
            var constant2 = Expression.Constant(value2, value2.GetType());
            return Expression.AndAlso(//combinamos comparaciones para limite inf y superior
                   Expression.GreaterThanOrEqual(property, constant),
                   Expression.LessThanOrEqual(property, constant2));
        }   
}
}