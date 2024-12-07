using Application.Interfaces;
using Application.Models.Export;
using Microsoft.AspNetCore.Mvc;


namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportGenericController<T> : ControllerBase where T : class
    {
        private readonly IExportService<T> _exportService;

        public ExportGenericController(IExportService<T> exportService)
        {
            _exportService = exportService;
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export(
            [FromQuery] ExportPropierty propertyName,
            [FromQuery] ExportOperation operation,
            [FromQuery] string value,
            [FromQuery] string value2 = null) 
        {
           var filterExpression = _exportService.BuildFilterExpression(propertyName, operation, value, value2);
           var fileBytes = await _exportService.ExportDataToExcel(filterExpression);
           return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FilteredData.xlsx");//contenido/tipo/nombre
        }

    }
}
