﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Articulos
{
    [Key]
    public int ArticuloId { get; set; }
    public int CarritoId { get; set; }
    [ForeignKey("CarritoId")]
    public virtual Carrito Carritos { get; set; }
    public int ProductoId { get; set; }
    [ForeignKey("ProductoId")]
    public virtual Productos Producto { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Cantidad { get; set; }
}
