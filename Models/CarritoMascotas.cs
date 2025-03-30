using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models
{
    public class CarritoMascotas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarritoMascotaId { get; set; }

        [Required(ErrorMessage = "El nombre de la mascota obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string? NombreMascota { get; set; }

        [Required]
        public int MascotaId { get; set; }
        [ForeignKey("MascotaId")]
        public virtual Mascotas Mascota { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

    }
}