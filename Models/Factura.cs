using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models
{
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FacturaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        public int ProductoId { get; set; }
        
        [Required]
        public int CarritoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Productos? Producto { get; set; }

        [ForeignKey("CarritoId")]
        public virtual Carrito? Carritos { get; set; }

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