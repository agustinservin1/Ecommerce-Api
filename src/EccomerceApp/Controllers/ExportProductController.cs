using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportProductController : ExportGenericController<Product>
    {
        public ExportProductController(IExportService<Product> exportService) : base(exportService)
        {
        }
    }
}
