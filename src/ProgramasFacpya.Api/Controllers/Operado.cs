using Microsoft.AspNetCore.Mvc;

namespace ProgramasFacpya.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Operado : ControllerBase
{

    [HttpGet("Suma/{Numero1:int}/{Numero2:int}")]
    public IActionResult Suma(int Numero1, int Numero2)
    {
        return Ok(Numero1 + Numero2);
    }


    [HttpGet("Resta/{Numero1:int}/{Numero2:int}")]
    public IActionResult Resta(int Numero1, int Numero2)
    {
        return Ok(Numero1 - Numero2);
    }
}
