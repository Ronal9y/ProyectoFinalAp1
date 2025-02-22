using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models
{
    public class ArticuloMascotas
    {
        [Key]
        public int Id { get; set; }
        public int CarritoId { get; set; }
        [ForeignKey("CarritoId")]
        public virtual Carrito Carritos { get; set; }
        public int MascotaId { get; set; }
        [ForeignKey("MascotaId")]
        public virtual Mascotas Mascota { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

    }
}
