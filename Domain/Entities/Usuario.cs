using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Usuario
    {
        [Key]
        public int PkUsuario {  get; set; }

        public string Nombre { get; set; }

        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [ForeignKey("Roles")]
        public int FkRol { get; set; }

        public Rol Roles { get; set; }

    }
}
