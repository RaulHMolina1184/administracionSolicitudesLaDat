using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entities;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic
{
    public class UsuarioService
    {
        private readonly UsuarioDA _usuarioDA;
        private readonly EncryptionService _encryptionService;

        // Constructor que recibe para obtener la cadena de conexión y el servicio de encriptación (key)
        public UsuarioService(UsuarioDA usuarioDA, EncryptionService encryptionService)
        {
            _usuarioDA = usuarioDA;
            _encryptionService = encryptionService;
        }
        // Método para validar login
        public bool Login(string nombreUsuario, string passwordInput)
        {
            var credentials = _usuarioDA.VerifyUsuario(nombreUsuario); // Llama sp hacía la base de datos

            // Si no existe o está bloqueado
            if (credentials.Encontrado != 1 || credentials.ContraseniaCifrada == null || credentials.Nonce == null)
            {
                return false;
            }

            // Verificar contraseña
            bool esValida = _encryptionService.VerifyPassword(credentials.ContraseniaCifrada, credentials.Nonce, passwordInput);

            if (esValida)
            {
                _usuarioDA.ReiniciarIntentos(nombreUsuario); // Resetear contador e intento
                return true;
            }
            else
            {
                _usuarioDA.RegistrarIntentoFallido(nombreUsuario); // Incrementar contador y bloquear si corresponde
                return false;
            }
        }
    }
}
