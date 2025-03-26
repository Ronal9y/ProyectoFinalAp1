using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Citas
{
    [Key]
    public int CitaId { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "´Debe introducir el nombre de su mascota.")]
    public string? NombreMascota { get; set; }  
    public int EmpleadoId { get; set; }
    [ForeignKey("EmpleadoId")]
    public virtual Empleados Empleado { get; set; }

}
