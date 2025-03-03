﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string? Nombre { get; set; }
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }
    [Required(ErrorMessage = "La Descripcion es obligatorio.")]
    public string? Descripcion { get; set; }
    [Required(ErrorMessage = "La imagen es obligatoria.")]
    [Url(ErrorMessage = "Debe proporcionar una URL válida para la foto del producto.")]
    public string? ImagenURL { get; set; }
    [Required(ErrorMessage = "El campo Precio es obligatorio.")]
    public decimal? Precio { get; set; }
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Cantidad { get; set; }
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public ProductoCategorias ProductoCategoria { get; set; }

    public int ProveedorId { get; set; }
    [ForeignKey("ProveedorId")]
    public virtual Proveedores Proveedor { get; set; }
}
