using ProyectoFinalAp1.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ProyectoFinalAp1.Models;
public class Carrito
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Asegura que CarritoId sea una columna de identidad
    public int CarritoId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    public int Cantidad { get; set; }

    [ForeignKey("ProductoId")]
    public int ProductoId { get; set; }

    public virtual Productos Producto { get; set; } // Relación con Productos
}