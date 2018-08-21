using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases.DTOs
{
    public class PedidoDTO
    {
        public int Id_pedido { get; set; }
        public string Referencia { get; set; }
        public System.DateTime Fecha { get; set; }
        public int FIdRestaurante { get; set; }
        public string NombreRestaurante { get; set; }
        public int FIdPersona { get; set; }
        public string NombrePersona { get; set; }
    }
}
