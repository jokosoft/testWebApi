using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases.DTOs
{
    public class ProductoDTO
    {
        public int Id_prod { get; set; }
        public string Nombre_prod { get; set; }
        public int FidFamilia { get; set; }
        public string NombreFamilia { get; set; }
        public decimal Precio { get; set; }

    }
}
