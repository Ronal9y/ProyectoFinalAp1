using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Models.Dtos
{
    public class ArticulosDto
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string ProductoDescripcion { get; set; }
        public string ProductoImageURL { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioTotal { get; set; }
        public int Cantidad { get; set; }
    }
}