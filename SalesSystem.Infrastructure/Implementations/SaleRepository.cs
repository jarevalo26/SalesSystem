using Microsoft.EntityFrameworkCore;
using SalesSystem.Domain.Entities;
using SalesSystem.Infrastructure.Contexts;
using SalesSystem.Infrastructure.Interfaces;

namespace SalesSystem.Infrastructure.Implementations
{
    public class SaleRepository : GenericRepository<Venta>, ISaleRepository
    {
        private readonly VentasDbContext _dbContext;

        public SaleRepository(VentasDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Register(Venta entity)
        {
            Venta sale = new();
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                foreach (DetalleVenta detail in entity.DetalleVenta)
                {
                    Producto product = _dbContext.Producto.Where(p => p.IdProducto == detail.IdProducto).First();
                    product.Stock -= detail.Cantidad;
                    _dbContext.Producto.Update(product);
                }
                await _dbContext.SaveChangesAsync();

                NumeroCorrelativo numCorrelativo = _dbContext.NumeroCorrelativo.Where(n => n.Gestion == "venta").First();
                numCorrelativo.UltimoNumero++;
                numCorrelativo.FechaActualizacion = DateTime.Now;
                _dbContext.NumeroCorrelativo.Update(numCorrelativo);
                await _dbContext.SaveChangesAsync();

                string ceros = string.Concat(Enumerable.Repeat("0", numCorrelativo.CantidadDigitos!.Value));
                string saleNum = ceros + numCorrelativo.UltimoNumero.ToString();
                saleNum = saleNum.Substring(saleNum.Length - numCorrelativo.CantidadDigitos.Value, numCorrelativo.CantidadDigitos.Value);

                entity.NumeroVenta = saleNum;
                await _dbContext.Venta.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                sale = entity;
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            return sale;
        }

        public async Task<List<DetalleVenta>> Report(DateTime initialDate, DateTime finalDate)
        {
            List<DetalleVenta> list = await _dbContext.DetalleVenta
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(u => u!.IdUsuarioNavigation)
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(t => t!.IdTipoDocumentoVentaNavigation)
                .Where(dv => 
                    dv.IdVentaNavigation!.FechaRegistro!.Value.Date >= initialDate && 
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= finalDate)
                .ToListAsync();

            return list;
        }
    }
}
