using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.dto
{
    public class UsuarioDto
    {
        public int PkUsuario;

        public string Nombre { get; set; }

        public string UserName { get; set; }

        // En una actualización, null o vacío conserva la contraseña actual.
        public string? Password { get; set; }

        public int FkRol { get; set; }
    }
}
