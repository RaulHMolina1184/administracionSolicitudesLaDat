using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Usuario
    {
        public string UsuarioID { get; set; } // id_usuario
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public byte[] ContraseniaCifrada { get; set; }
        public byte[] Nonce { get; set; }
        public EstadoUsuario Estado { get; set; } // enum convertido a Enum

    }
}
