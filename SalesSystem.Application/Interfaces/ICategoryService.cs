using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Categoria>> List();
        Task<Categoria> Create(Categoria entity);
        Task<Categoria> Edit(Categoria entity);
        Task<bool> Delete(int categoryId);
    }
}
