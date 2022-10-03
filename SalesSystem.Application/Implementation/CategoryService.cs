using Microsoft.VisualBasic;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Categoria> _repository;

        public CategoryService(IGenericRepository<Categoria> repository)
        {
            _repository = repository;
        }

        public async Task<List<Categoria>> List()
        {
            IQueryable<Categoria> query = await _repository.GetAll();
            return query.ToList();
        }

        public async Task<Categoria> Create(Categoria entity)
        {
            try
            {
                Categoria category = await _repository.Create(entity);
                if (category.IdCategoria == 0)
                    throw new TaskCanceledException("No se pudo crear la categoria");

                return category;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Categoria> Edit(Categoria entity)
        {
            try
            {
                Categoria category = await _repository.Get(c => c.IdCategoria == entity.IdCategoria);
                category.Descripcion = entity.Descripcion;
                category.EsActivo = entity.EsActivo;
                bool response = await _repository.Update(entity);
                if (!response)
                    throw new TaskCanceledException("No se pudo modificar la categoria");

                return category;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int categoryId)
        {
            try
            {
                Categoria category = await _repository.Get(c => c.IdCategoria == categoryId);
                if (category == null)
                    throw new TaskCanceledException("La categoria no existe");

                bool response = await _repository.Delete(category);
                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
