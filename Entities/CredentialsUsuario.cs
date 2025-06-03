using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CredentialsUsuario
    {
        public string NombreUsuario { get; set; } // Nombre de usuario
        public byte[] ContraseniaCifrada { get; set; } // Contraseña cifrada
        public byte[] Nonce { get; set; } // Nonce para cifrado
        public int Encontrado { get; set; } // Indicador de si se encontró el usuario (1 = encontrado, 0 = no encontrado)
    }
}
