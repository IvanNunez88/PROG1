using Dapper;
using FluentValidation;
using ProgramasFacpya.Api.Common;
using ProgramasFacpya.Api.Features.Carreras.Dominio.Dto;
using ProgramasFacpya.Api.Features.Carreras.Dominio.Entidad;
using ProgramasFacpya.Api.Data;

namespace ProgramasFacpya.Api.Features.Carreras;

public class CarreraService(DbContext db, IValidator<CarreraDTO> validator)
{
    private readonly DbContext _db = db;
    private readonly IValidator<CarreraDTO> _validator = validator;

    public async Task<ApiResponse<IEnumerable<Carrera>>> ObtenerTodos()
    {
        using var connection = _db.CreateConnection();
        var sql = @"SELECT IdCarrera,
                           Descripcion
                    FROM CARRERA
                    ORDER BY Descripcion";

        var carreras = await connection.QueryAsync<Carrera>(sql);
        return ApiResponse<IEnumerable<Carrera>>.Ok(carreras);
    }

    public async Task<ApiResponse<Carrera>> ObtenerPorId(byte idCarrera)
    {
        using var connection = _db.CreateConnection();
        var sql = @"SELECT IdCarrera, Descripcion
                    FROM CARRERA
                    WHERE IdCarrera = @IdCarrera";

        var carrera = await connection.QueryFirstOrDefaultAsync<Carrera>(sql, new { IdCarrera = idCarrera });

        if (carrera == null)
            return ApiResponse<Carrera>.Error("Carrera no encontrada");

        return ApiResponse<Carrera>.Ok(carrera);
    }

    public async Task<ApiResponse<byte>> Crear(CarreraDTO dto)
    {
        var validacion = await _validator.ValidateAsync(dto);
        if (!validacion.IsValid)
            return ApiResponse<byte>.Error(validacion.Errors.First().ErrorMessage);

        using var connection = _db.CreateConnection();
        if (await ExisteDescripcion(connection, dto.Descripcion))
            return ApiResponse<byte>.Error("La carrera ya existe");

        var sql = @"INSERT INTO CARRERA (Descripcion)
                    VALUES (@Descripcion);
                    SELECT CAST(SCOPE_IDENTITY() AS TINYINT);";

        var idCarrera = await connection.ExecuteScalarAsync<byte>(sql, new { dto.Descripcion });
        return ApiResponse<byte>.Ok(idCarrera, "Carrera registrada");
    }

    public async Task<ApiResponse<string>> Actualizar(byte idCarrera, CarreraDTO dto)
    {
        var validacion = await _validator.ValidateAsync(dto);
        if (!validacion.IsValid)
            return ApiResponse<string>.Error(validacion.Errors.First().ErrorMessage);

        using var connection = _db.CreateConnection();
        if (await ExisteDescripcion(connection, dto.Descripcion, idCarrera))
            return ApiResponse<string>.Error("La carrera ya existe");

        var sql = @"UPDATE CARRERA SET Descripcion = @Descripcion
                    WHERE IdCarrera = @IdCarrera";
        var rows = await connection.ExecuteAsync(sql, new { IdCarrera = idCarrera, dto.Descripcion });

        if (rows == 0) return ApiResponse<string>.Error("Carrera no encontrada");

        return ApiResponse<string>.Ok("Carrera actualizada");
    }

    public async Task<ApiResponse<string>> Eliminar(byte idCarrera)
    {
        using var connection = _db.CreateConnection();

        // ALUMNO tiene FK a CARRERA, no se puede borrar si tiene alumnos
        var tieneAlumnos = await connection.ExecuteScalarAsync<bool>(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM ALUMNO WHERE IdCarrera = @IdCarrera) THEN 1 ELSE 0 END",
            new { IdCarrera = idCarrera });

        if (tieneAlumnos)
            return ApiResponse<string>.Error("No se puede eliminar, la carrera tiene alumnos registrados");

        string SqlScript = @"DELETE FROM CARRERA WHERE IdCarrera = @IdCarrera";

        var rows = await connection.ExecuteAsync(SqlScript, new { IdCarrera = idCarrera });

        if (rows == 0)
            return ApiResponse<string>.Error("Carrera no encontrada");

        return ApiResponse<string>.Ok("Carrera eliminada");
    }

    // idCarrera excluye a la propia carrera al actualizar
    private static async Task<bool> ExisteDescripcion(System.Data.IDbConnection connection, string descripcion, byte idCarrera = 0)
    {
        var existe = await connection.QueryFirstOrDefaultAsync<byte?>(
            "SELECT IdCarrera FROM CARRERA WHERE Descripcion = @Descripcion AND IdCarrera <> @IdCarrera",
            new { Descripcion = descripcion, IdCarrera = idCarrera });

        return existe != null;
    }
}
