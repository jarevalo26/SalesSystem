using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Interfaces
{
    public interface ISalesDocumentTypeService
    {
        Task<List<TipoDocumentoVenta>> List();
    }
}
