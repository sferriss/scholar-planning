using Microsoft.AspNetCore.Mvc;
using Scholar.Planning.Domain.Services;

namespace Scholar.Planning.Api.Controllers;

[ApiController]
[Route("file")]
public class FileController(ILogger<FileController> logger, IPdfGenerator pdfGenerator)
    : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var pdfBytes = pdfGenerator.GenerateFile();
        
        return File(pdfBytes, "application/pdf", "documento.pdf");
    }
}