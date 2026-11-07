namespace ProgramasFacpya.Api.Features.Maestros.Dominio.Entidad;

public class Maestro
{
    public int IdMaestro { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string APaterno { get; set; } = string.Empty;
    public string AMaterno { get; set; } = string.Empty;
    public DateTime FecRegistro { get; set; }
}
