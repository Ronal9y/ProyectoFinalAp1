using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Usuarios
{
    [Key]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "Favor colocar los apellidos.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Los apellidos solo pueden contener letras.")]
    public string? Apellido { get; set; }

   
    [DataType(DataType.Date)]
    public DateTime FechaIngreso { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Favor colocar la direccion es obligatoria.")]
    [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s,.#-]+$", ErrorMessage = "La dirección contiene caracteres no válidos.")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "Favor colocar el teléfono.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe contener exactamente 10 dígitos.")]
    public int? Telefono { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
    public string? Email { get; set; }

    public virtual ICollection<Mascotas> Mascota { get; set; } = new List<Mascotas>();
   
}
