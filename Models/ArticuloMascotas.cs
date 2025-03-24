using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class ArticuloMascotas
{
    [Key]
    public int Id { get; set; }

    public int CarritoId { get; set; } // Solo una referencia a Carritos
    [ForeignKey("CarritoId")]
    public virtual Carrito Carrito { get; set; } // Relación con Carrito

    public int MascotaId { get; set; }
    [ForeignKey("MascotaId")]
    public virtual Mascotas Mascota { get; set; }

    public int Cantidad { get; set; }
}