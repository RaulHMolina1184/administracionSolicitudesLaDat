using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic;

namespace administracionSolicitudesLaDat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UsuarioService _usuarioService;

        [BindProperty]
        public string NombreUsuario { get; set; }

        [BindProperty]
        public string Contrasenia { get; set; }

        public string Mensaje { get; set; }
        public string MensajeInfo { get; set; }


        public IndexModel(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult OnPost()
        {
            bool resultado = _usuarioService.Login(NombreUsuario, Contrasenia); // hacia base de datos, envia solamente el usuario

            if (resultado) // Siempre espera un true
            {
                // guardar el usuario en sesi�n o redirigirlo
                return RedirectToPage("/Principal"); // P�gina de destino despu�s del inicio de sesi�n exitoso
            }

            Mensaje = "Usuario y/o\r\ncontrase�a incorrectos.";
            return Page();
        }
    }
}