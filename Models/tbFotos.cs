//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Adventure_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbFotos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbFotos()
        {
            this.tbComentarios = new HashSet<tbComentarios>();
            this.tbFavoritos = new HashSet<tbFavoritos>();
        }
    
        public int id { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public System.DateTime fechaModificacion { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string pathFoto { get; set; }
        public int id_user { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbComentarios> tbComentarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbFavoritos> tbFavoritos { get; set; }
        public virtual tbUsuarios tbUsuarios { get; set; }
    }
}
