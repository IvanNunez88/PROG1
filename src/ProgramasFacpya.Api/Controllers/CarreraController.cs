using Microsoft.AspNetCore.Mvc;
using ProgramasFacpya.Api.Features.Carreras;
using ProgramasFacpya.Api.Features.Carreras.Dominio.Dto;

namespace ProgramasFacpya.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarreraController(CarreraService service) : ControllerBase
{
    private readonly CarreraService _service = service;

    [HttpGet("ObtenerTodos")]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _service.ObtenerTodos());
    }

    [HttpGet("ObtenerPorId/{idCarrera:int}")]
    public async Task<IActionResult> ObtenerPorId(byte idCarrera)
    {
        return Ok(await _service.ObtenerPorId(idCarrera));
    }

    [HttpPost("Crear")]
    public async Task<IActionResult> Crear([FromBody] CarreraDTO dto)
    {
        return Ok(await _service.Crear(dto));
    }

    [HttpPut("Actualizar/{idCarrera}")]
    public async Task<IActionResult> Actualizar(byte idCarrera, [FromBody] CarreraDTO dto)
    {
        return Ok(await _service.Actualizar(idCarrera, dto));
    }

    [HttpDelete("Eliminar/{idCarrera}")]
    public async Task<IActionResult> Eliminar(byte idCarrera)
    {
        return Ok(await _service.Eliminar(idCarrera));
    }
}
