using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Mascotas
{
    [Key]
    public int MascotaId { get; set; }

    [Required(ErrorMessage = "El campo Tipo es obligatorio.")]
    public string? Tipo { get; set; }

    [Required(ErrorMessage = "El campo Raza es obligatorio.")]
    public string? Raza { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaDeNacimiento { get; set; }

    [Required(ErrorMessage = "El campo Precio es obligatorio.")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Cantidad { get; set; }
    [Required(ErrorMessage = "La imagen es obligatoria.")]

    [NotMapped]
    public int CantidadSeleccionada { get; set; } = 1;
    public string ImageURL { get; set; }
    public int DonadorId { get; set; }
    [ForeignKey("DonadorId")]
    public virtual Donador Donador { get; set; }

    public virtual ICollection<CarritoMascotas> CarritoMascotas { get; set; } = new List<CarritoMascotas>();
    public virtual ICollection<Citas> Cita { get; set; } = new List<Citas>();

}
