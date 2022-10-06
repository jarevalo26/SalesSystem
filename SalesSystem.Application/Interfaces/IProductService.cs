using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<Producto>> List();
        Task<Producto> Create(Producto entity, Stream? image = null, string imageName = "");
        Task<Producto> Edit(Producto entity, Stream? image = null, string imageName = "");
        Task<bool> Delete(int productId);
    }
}
