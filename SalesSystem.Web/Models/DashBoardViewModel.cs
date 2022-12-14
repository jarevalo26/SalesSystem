namespace SalesSystem.Web.Models
{
    public class DashBoardViewModel
    {
        public int TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalProductos { get; set; }
        public int TotalCategorias { get; set; }
        public List<VentasSemanaViewModel>? VentasUltimaSemana { get; set; }
        public List<ProductosSemanaViewModel>? ProductosTopUltimaSemana { get; set; }
    }
}
