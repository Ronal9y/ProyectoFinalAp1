using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Proveedores
{
    [Key]
    public int ProveedorId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string? Nombre { get; set; }
    [Required(ErrorMessage = "El campo fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaDeEntrega { get; set; }

    [Required(ErrorMessage = "Favor colocar la dirección.")]
    [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s,.#-]+$", ErrorMessage = "La dirección contiene caracteres no válidos.")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "Favor colocar el teléfono.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de teléfono debe contener exactamente 10 dígitos.")]
    public int? Telefono { get; set; }
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
    public string? Email { get; set; }

}
