using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Citas
{
    [Key]
    public int CitaId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now; 
    public int MascotaId { get; set; }
    [ForeignKey("MascotaId")]
    public virtual Mascotas Mascota { get; set; }

    public int EmpleadoId { get; set; }
    [ForeignKey("EmpleadoId")]
    public virtual Empleados Empleado { get; set; }

}
