using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Carrito
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CarritoId { get; set; }

    [Required]
    public int ProductoId { get; set; }
    [ForeignKey("ProductoId")]

    public virtual Productos Producto { get; set; }

  
    public int Cantidad { get; set; }
    public virtual ICollection<Factura> Facturas { get; set; }
}
