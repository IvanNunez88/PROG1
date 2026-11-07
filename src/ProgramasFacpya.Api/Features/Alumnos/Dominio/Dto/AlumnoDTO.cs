namespace ProgramasFacpya.Api.Features.Alumnos.Dominio.Dto;

// Se usa tanto para Crear como para Actualizar (IdAlumno es IDENTITY y va en la ruta)
public class AlumnoDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string APaterno { get; set; } = string.Empty;
    public string AMaterno { get; set; } = string.Empty;
    public byte IdCarrera { get; set; }
}
