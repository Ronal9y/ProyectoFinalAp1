using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Carrito
{
    [Key]
    public int CarritoId { get; set; }
    public int UsuarioId { get; set; }
    [ForeignKey("UsuarioId")]
    public virtual Usuarios Usuario { get; set; }
    public DateTime Fecha { get; set; }

    public virtual ICollection<Articulos> Articulo { get; set; } = new List<Articulos>();
    public virtual ICollection<ArticuloMascotas> ArticuloMascota { get; set; } = new List<ArticuloMascotas>();
}
