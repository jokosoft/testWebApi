//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModeloPedidos.Clases
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedidos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pedidos()
        {
            this.ProductosPedidos = new HashSet<ProductosPedidos>();
        }
    
        public int Id_pedido { get; set; }
        public string Referencia { get; set; }
        public System.DateTime Fecha { get; set; }
        public int FIdRestaurante { get; set; }
        public int FIdPersona { get; set; }
    
        public virtual Personas Personas { get; set; }
        public virtual Restaurantes Restaurantes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductosPedidos> ProductosPedidos { get; set; }
    }
}
