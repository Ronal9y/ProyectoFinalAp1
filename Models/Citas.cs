using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Citas
{
    [Key]
    public int CitaId { get; set; }

    [Required(ErrorMessage = "Favor colocar su nombre")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre  solo pueden contener letras.")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "La fecha para su cita  es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "´Debe escribir una descricion sobre su cita")]
    public string? Descripcion { get; set; }


    [Required(ErrorMessage = "´Debe introducir el nombre de su mascota.")]
    public string? NombreMascota { get; set; }  
    public int EmpleadoId { get; set; }
    [ForeignKey("EmpleadoId")]
    public virtual Empleados Empleado { get; set; }

}
