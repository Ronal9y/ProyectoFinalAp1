using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models
{
    public class FacturaMascota
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FacturaMascotaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        public int MascotaId { get; set; }
        [ForeignKey("MascotaId")]
        public virtual Mascotas? Mascotas { get; set; }

        public int CarritoMascotaId { get; set; }

        [ForeignKey("CarritoMascotaId")]
        public virtual CarritoMascotas? CarritoMascotas { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public double Precio { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public double Subtotal { get; set; }

    }
}