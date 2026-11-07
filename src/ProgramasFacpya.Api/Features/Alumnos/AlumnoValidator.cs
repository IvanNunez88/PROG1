using FluentValidation;
using ProgramasFacpya.Api.Features.Alumnos.Dominio.Dto;

namespace ProgramasFacpya.Api.Features.Alumnos;

public class AlumnoValidator : AbstractValidator<AlumnoDTO>
{
    public AlumnoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .Must(TenerMinimoTresLetras).WithMessage("El nombre debe tener al menos 3 caracteres")
            .Must(SoloLetras).WithMessage("El nombre solo debe contener letras")
            .MaximumLength(50).WithMessage("El nombre no puede tener mas de 50 caracteres");

        RuleFor(x => x.APaterno)
            .NotEmpty().WithMessage("El apellido paterno es requerido")
            .Must(TenerMinimoTresLetras).WithMessage("El apellido paterno debe tener al menos 3 caracteres")
            .Must(SoloLetras).WithMessage("El apellido paterno solo debe contener letras")
            .MaximumLength(80).WithMessage("El apellido paterno no puede tener mas de 80 caracteres");

        RuleFor(x => x.AMaterno)
            .NotEmpty().WithMessage("El apellido materno es requerido")
            .Must(TenerMinimoTresLetras).WithMessage("El apellido materno debe tener al menos 3 caracteres")
            .Must(SoloLetras).WithMessage("El apellido materno solo debe contener letras")
            .MaximumLength(80).WithMessage("El apellido materno no puede tener mas de 80 caracteres");

        RuleFor(x => x.IdCarrera)
            .GreaterThan((byte)0).WithMessage("La carrera es requerida");
    }

    // Los espacios no cuentan (ej. "  a b " no es valido)
    private static bool TenerMinimoTresLetras(string texto) =>
        texto.Replace(" ", "").Length >= 3;

    // Permite espacios para nombres compuestos (ej. "Juan Carlos")
    private static bool SoloLetras(string texto) =>
        texto.Replace(" ", "").All(char.IsLetter);
}