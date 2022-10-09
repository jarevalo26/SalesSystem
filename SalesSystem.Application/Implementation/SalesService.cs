using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using System.Globalization;

namespace SalesSystem.Application.Implementation
{
    public class SalesService : ISalesService
    {
        private readonly IGenericRepository<Producto> _product;
        private readonly ISaleRepository _sale;

        public SalesService(
            IGenericRepository<Producto> product,
            ISaleRepository sale)
        {
            _product = product;
            _sale = sale;
        }

        public async Task<List<Producto>> GetProducts(string search)
        {
            IQueryable<Producto> query = await _product.GetAll(p =>
            p.EsActivo == true &&
            p.Stock > 0 &&
            string.Concat(p.CodigoBarra, p.Marca, p.Descripcion).Contains(search));

            return query.Include(c => c.IdCategoriaNavigation).ToList();
        }

        public async Task<Venta> Register(Venta entity)
        {
            try
            {
                return await _sale.Register(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Venta>> SalesHistory(string salesNumber, string? initialDate, string? finalDate)
        {
            IQueryable<Venta> query = await _sale.GetAll();
            _ = initialDate is null ? "" : initialDate;
            _ = finalDate is null ? "" : finalDate;

            if (initialDate != "" && finalDate != "")
            {
                DateTime iDate = DateTime.ParseExact(initialDate!, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime fDate = DateTime.ParseExact(finalDate!, "dd/MM/yyyy", new CultureInfo("es-CO"));
                _ = query
                    .Where(v =>
                        v.FechaRegistro!.Value.Date >= iDate &&
                        v.FechaRegistro.Value.Date <= fDate)
                    .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dv => dv.DetalleVenta);
            }
            else
            {
                _ = query.Where(v => v.NumeroVenta == salesNumber)
                    .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dv => dv.DetalleVenta);
            }
            return query.ToList();
        }

        public async Task<Venta> Details(string salesNumber)
        {
            IQueryable<Venta> query = await _sale.GetAll(v => v.NumeroVenta == salesNumber);

            Venta? sale = query
                .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                .Include(u => u.IdUsuarioNavigation)
                .Include(dv => dv.DetalleVenta).FirstOrDefault();

            return sale!;
        }

        public async Task<List<DetalleVenta>> Report(string initialDate, string finalDate)
        {
            DateTime iDate = DateTime.ParseExact(initialDate!, "dd/MM/yyyy", new CultureInfo("es-CO"));
            DateTime fDate = DateTime.ParseExact(finalDate!, "dd/MM/yyyy", new CultureInfo("es-CO"));

            List<DetalleVenta> list = await _sale.Report(iDate, fDate);
            return list;
        }
    }
}
