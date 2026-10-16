using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Rol
    {
        [Key]
        public int PkRol { get; set;  }
        public string Nombre { get; set; }
        public bool Estatus { get; set; }

    }
}
