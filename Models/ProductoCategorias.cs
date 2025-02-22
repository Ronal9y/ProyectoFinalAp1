using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class ProductoCategorias
{
    [Key]
    public int CategoriaId { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string? Nombre { get; set; }

    public string? IconCSS { get; set; }

    public virtual ICollection<Productos> Producto { get; set; } = new List<Productos>();
}
