using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Rol> _repository;

        public RoleService(IGenericRepository<Rol> repository)
        {
            _repository = repository;
        }

        public async Task<List<Rol>> Roles()
        {
            IQueryable<Rol> roles = await _repository.GetAll();
            return roles.ToList();
        }
    }
}
