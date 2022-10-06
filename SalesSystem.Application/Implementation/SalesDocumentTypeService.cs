using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Implementation
{
    public class SalesDocumentTypeService : ISalesDocumentTypeService
    {
        private readonly IGenericRepository<TipoDocumentoVenta> _repository;

        public SalesDocumentTypeService(IGenericRepository<TipoDocumentoVenta> repository)
        {
            _repository = repository;
        }

        public async Task<List<TipoDocumentoVenta>> List()
        {
            IQueryable<TipoDocumentoVenta> query = await _repository.GetAll();
            return query.ToList();
        }
    }
}
