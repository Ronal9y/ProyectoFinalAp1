using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProyectoFinalAp1.Models.Dtos
{
    public class ProductosDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenURL { get; set; }
        public decimal? Precio { get; set; }
        public int Cantidad { get; set; }
        public int CategoriaId { get; set; }
        public string NombreCategoria { get; set; }

    }
}
