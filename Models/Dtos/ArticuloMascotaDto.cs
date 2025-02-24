using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Models.Dtos
{
    public class ArticuloMascotaDto
    {

        public int Id { get; set; }
        public int CarritoId { get; set; }
        public int MascotaId { get; set; }
        public string RazaMascota { get; set; }
        public string MascotaImageURL { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioTotal { get; set; }
        public int Cantidad { get; set; }

    }
}
