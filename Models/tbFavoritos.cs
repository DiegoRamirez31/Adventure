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
    
    public partial class tbFavoritos
    {
        public int id { get; set; }
        public int id_foto { get; set; }
        public int id_user { get; set; }
    
        public virtual tbFotos tbFotos { get; set; }
        public virtual tbUsuarios tbUsuarios { get; set; }
    }
}