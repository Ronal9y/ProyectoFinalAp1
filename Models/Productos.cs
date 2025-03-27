using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]

    public string? Nombre { get; set; }

    [Required(ErrorMessage = "La Descripcion es obligatorio.")]
    [StringLength(150, ErrorMessage = "La descripcion no puede exceder los 150 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La imagen es obligatoria.")]
 

    public string? ImagenURL { get; set; }

    [Required(ErrorMessage = "El campo Precio es obligatorio.")]

    public double Precio { get; set; }
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]

    public int Stock { get; set; }

    [NotMapped]
    public int CantidadSeleccionada { get; set; } = 1;
    public string? TipoCategoria { get; set; }
    public int ProveedorId { get; set; }
    [ForeignKey("ProveedorId")]
    public virtual Proveedores Proveedores { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
}
