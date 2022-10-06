using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Interfaces
{
    public interface ISalesService
    {
        Task<List<Producto>> GetProducts(string search);
        Task<Venta> Register(Venta entity);
        Task<List<Venta>> SalesHistory(string salesNumber, string initialDate, string finalDate);
        Task<Venta> Details(string salesNumber);
        Task<List<DetalleVenta>> Report(string initialDate, string finalDate);
    }
}
