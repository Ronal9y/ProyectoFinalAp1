using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models
{
    public class CarritoMascotas
    {
        [Key]
        public int CarritoMascotaId { get; set; }

        [Required]
        public int MascotaId { get; set; }
        [ForeignKey("MascotaId")]
        public virtual Mascotas Mascota { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }
       
    }
}
