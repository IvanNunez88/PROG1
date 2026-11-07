using Dapper;
using FluentValidation;
using ProgramasFacpya.Api.Common;
using ProgramasFacpya.Api.Features.Maestros.Dominio.Dto;
using ProgramasFacpya.Api.Features.Maestros.Dominio.Entidad;
using ProgramasFacpya.Api.Data;

namespace ProgramasFacpya.Api.Features.Maestros;

public class MaestroService(DbContext db, IValidator<MaestroDTO> validator)
{
    private readonly DbContext _db = db;
    private readonly IValidator<MaestroDTO> _validator = validator;

    public async Task<ApiResponse<IEnumerable<Maestro>>> ObtenerTodos()
    {
        using var connection = _db.CreateConnection();
        var sql = @"SELECT IdMaestro,
                           Nombre,
                           APaterno,
                           AMaterno,
                           FecRegistro
                    FROM MAESTRO
                    ORDER BY APaterno, AMaterno, Nombre";

        var maestros = await connection.QueryAsync<Maestro>(sql);
        return ApiResponse<IEnumerable<Maestro>>.Ok(maestros);
    }

    public async Task<ApiResponse<Maestro>> ObtenerPorId(int idMaestro)
    {
        using var connection = _db.CreateConnection();
        var sql = @"SELECT IdMaestro, Nombre, APaterno, AMaterno, FecRegistro
                    FROM MAESTRO
                    WHERE IdMaestro = @IdMaestro";

        var maestro = await connection.QueryFirstOrDefaultAsync<Maestro>(sql, new { IdMaestro = idMaestro });

        if (maestro == null)
            return ApiResponse<Maestro>.Error("Maestro no encontrado");

        return ApiResponse<Maestro>.Ok(maestro);
    }

    public async Task<ApiResponse<int>> Crear(MaestroDTO dto)
    {
        var validacion = await _validator.ValidateAsync(dto);
        if (!validacion.IsValid)
            return ApiResponse<int>.Error(validacion.Errors.First().ErrorMessage);

        using var connection = _db.CreateConnection();
        // IdMaestro es IDENTITY y FecRegistro se llena solo con el DEFAULT GETDATE() de la tabla
        var sql = @"INSERT INTO MAESTRO (Nombre, APaterno, AMaterno)
                    VALUES (@Nombre, @APaterno, @AMaterno);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var idMaestro = await connection.ExecuteScalarAsync<int>(sql, new { dto.Nombre, dto.APaterno, dto.AMaterno });
        return ApiResponse<int>.Ok(idMaestro, "Maestro registrado");
    }

    public async Task<ApiResponse<string>> Actualizar(int idMaestro, MaestroDTO dto)
    {
        var validacion = await _validator.ValidateAsync(dto);
        if (!validacion.IsValid)
            return ApiResponse<string>.Error(validacion.Errors.First().ErrorMessage);

        using var connection = _db.CreateConnection();
        var sql = @"UPDATE MAESTRO SET Nombre = @Nombre,
                                       APaterno = @APaterno, AMaterno = @AMaterno
                    WHERE IdMaestro = @IdMaestro";
        var rows = await connection.ExecuteAsync(sql, new { IdMaestro = idMaestro, dto.Nombre, dto.APaterno, dto.AMaterno });

        if (rows == 0) return ApiResponse<string>.Error("Maestro no encontrado");

        return ApiResponse<string>.Ok("Maestro actualizado");
    }

    public async Task<ApiResponse<string>> Eliminar(int idMaestro)
    {
        using var connection = _db.CreateConnection();

        string SqlScript = @"DELETE FROM MAESTRO WHERE IdMaestro = @IdMaestro";

        var rows = await connection.ExecuteAsync(SqlScript, new { IdMaestro = idMaestro });

        if (rows == 0)
            return ApiResponse<string>.Error("Maestro no encontrado");

        return ApiResponse<string>.Ok("Maestro eliminado");
    }
}
