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
                // guardar el usuario en sesión o redirigirlo
                return RedirectToPage("/Principal"); // Página de destino después del inicio de sesión exitoso
            }

            Mensaje = "Usuario y/o\r\ncontraseña incorrectos.";
            return Page();
        }
    }
}