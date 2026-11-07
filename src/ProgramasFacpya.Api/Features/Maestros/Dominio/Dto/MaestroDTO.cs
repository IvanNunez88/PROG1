namespace ProgramasFacpya.Api.Features.Maestros.Dominio.Dto;

// Se usa tanto para Crear como para Actualizar (IdMaestro es IDENTITY y va en la ruta)
public class MaestroDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string APaterno { get; set; } = string.Empty;
    public string AMaterno { get; set; } = string.Empty;
}
