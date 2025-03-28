using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAp1.Models;

public class Donador
{
    [Key]
    public int DonadorId { get; set; }
  
    public string? Nombre { get; set; }

    public string? Direccion { get; set; }
 
    public string? Telefono { get; set; }

    public string? Email { get; set; }

  
}
