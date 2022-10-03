using SalesSystem.Domain.Entities;

namespace SalesSystem.Web.Models
{
    public class MenuViewModel
    {
        public string? Descripcion { get; set; }
        public string? Icono { get; set; }
        public string? Controlador { get; set; }
        public string? PaginaAccion { get; set; }

        public virtual ICollection<Menu>? SubMenus { get; set; }
    }
}
