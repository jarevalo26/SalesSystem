using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Infrastructure.Interfaces
{
    public interface ISaleRepository : IGenericRepository<Venta>
    {
        Task<Venta> Register(Venta entity);
        Task<List<DetalleVenta>> Report(DateTime initialDate, DateTime finalDate);
    }
}
