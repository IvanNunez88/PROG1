namespace ProgramasFacpya.Api.Features.Carreras.Dominio.Dto;

// Se usa tanto para Crear como para Actualizar (IdCarrera es IDENTITY y va en la ruta)
public class CarreraDTO
{
    public string Descripcion { get; set; } = string.Empty;
}
