namespace SalesSystem.Web.Models
{
    public class UsuarioLoginViewModel
    {
        public string? Correo { get; set; }
        public string? Clave { get; set; }
        public bool MantenerSesion { get; set; }
    }
}
