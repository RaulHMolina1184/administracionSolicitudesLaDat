using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class RegistroUsuario
    {
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasenia { get; set; } // Contraseña en texto plano (será cifrada luego)
    }
}
