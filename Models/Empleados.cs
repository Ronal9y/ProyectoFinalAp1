using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Empleados
{
    [Key]
    public int EmpleadoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string? Nombre { get; set; }
    [Required(ErrorMessage = "Favor colocar los apellidos.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Los apellidos solo pueden contener letras.")]
    public string? Apellido { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
    public string? Email { get; set; }
    [Required(ErrorMessage ="El campo cargo es obligatorio")]
    public string? Cargo { get; set; }
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaDeContratacion { get; set; } = DateTime.Now;   

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public decimal Salario { get; set; }

    public virtual ICollection<Citas> Cita { get; set; } = new List<Citas>();
}
