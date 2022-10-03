using AutoMapper;
using SalesSystem.Domain.Entities;
using SalesSystem.Web.Models;
using System.Globalization;

namespace SalesSystem.Web.Utilities.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolViewModel>().ReverseMap(); 
            #endregion Rol

            #region Usuario
            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(
                    d => d.EsActivo,
                    opt => opt.MapFrom(o => o.EsActivo == true ? 1 : 0))
                .ForMember(
                    d => d.NombreRol,
                    opt => opt.MapFrom(o => o.IdRolNavigation!.Descripcion));

            CreateMap<UsuarioViewModel, Usuario>()
                .ForMember(
                    d => d.EsActivo,
                    opt => opt.MapFrom(o => o.EsActivo == 1 ? true : false))
                .ForMember(
                    d => d.IdRolNavigation,
                    opt => opt.Ignore());
            #endregion Usuario

            #region Negocio
            CreateMap<Negocio, NegocioViewModel>()
                .ForMember(
                    d => d.PorcentajeImpuesto,
                    opt => opt.MapFrom(o => Convert.ToString(o.PorcentajeImpuesto!.Value, new CultureInfo("es-CO"))));

            CreateMap<NegocioViewModel, Negocio>()
                .ForMember(
                    d => d.PorcentajeImpuesto,
                    opt => opt.MapFrom(o => Convert.ToDecimal(o.PorcentajeImpuesto!.Value, new CultureInfo("es-CO"))));
            #endregion Negocio

            #region Categoria
            CreateMap<Categoria, CategoriaViewModel>()
                .ForMember(
                    d => d.EsActivo,
                    opt => opt.MapFrom(o => o.EsActivo == true ? 1 : 0));

            CreateMap<CategoriaViewModel, Categoria>()
                .ForMember(
                    d => d.EsActivo,
                    opt => opt.MapFrom(o => o.EsActivo == 1 ? true : false));
            #endregion Categoria

            #region Producto
            CreateMap<Producto, ProductoViewModel>()
                .ForMember(
                    d => d.EsActivo,
                    opt => opt.MapFrom(o => o.EsActivo == true ? 1 : 0))
                .ForMember(
                    d => d.NombreCategoria,
                    opt => opt.MapFrom(o => o.IdCategoriaNavigation!.Descripcion))
                .ForMember(
                    d => d.Precio,
                    opt => opt.MapFrom(o => Convert.ToString(o.Precio!.Value, new CultureInfo("es-CO"))));

            CreateMap<ProductoViewModel, Producto>()
                .ForMember(
                    d => d.EsActivo,
                    opt => opt.MapFrom(o => o.EsActivo == 1 ? true : false))
                .ForMember(
                    d => d.IdCategoriaNavigation,
                    opt => opt.Ignore())
                .ForMember(
                    d => d.Precio,
                    opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio, new CultureInfo("es-CO"))));
            #endregion Producto

            #region TipoDocumentoVenta
            CreateMap<TipoDocumentoVenta, TipoDocumentoVentaViewModel>().ReverseMap();
            #endregion TipoDocumentoVenta

            #region Venta
            CreateMap<Venta, VentaViewModel>()
                .ForMember(
                    d => d.TipoDocumentoVenta,
                    opt => opt.MapFrom(o => o.IdTipoDocumentoVentaNavigation!.Descripcion))
                .ForMember(
                    d => d.Usuario,
                    opt => opt.MapFrom(o => o.IdUsuarioNavigation!.Nombre))
                .ForMember(
                    d => d.SubTotal,
                    opt => opt.MapFrom(o => Convert.ToString(o.SubTotal!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.ImpuestoTotal,
                    opt => opt.MapFrom(o => Convert.ToString(o.ImpuestoTotal!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.Total,
                    opt => opt.MapFrom(o => Convert.ToString(o.Total!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.FechaRegistro,
                    opt => opt.MapFrom(o => o.FechaRegistro!.Value.ToString("dd/MM/yyyy")));

            CreateMap<VentaViewModel, Venta>()
                .ForMember(
                    d => d.SubTotal,
                    opt => opt.MapFrom(o => Convert.ToDecimal(o.SubTotal, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.ImpuestoTotal,
                    opt => opt.MapFrom(o => Convert.ToDecimal(o.ImpuestoTotal, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.Total,
                    opt => opt.MapFrom(o => Convert.ToDecimal(o.Total, new CultureInfo("es-CO"))));
            #endregion Venta

            #region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaViewModel>()
                .ForMember(
                    d => d.Precio,
                    opt => opt.MapFrom(o => Convert.ToString(o.Precio!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.Total,
                    opt => opt.MapFrom(o => Convert.ToString(o.Total!.Value, new CultureInfo("es-CO"))));

            CreateMap<DetalleVentaViewModel, DetalleVenta>()
                .ForMember(
                    d => d.Precio,
                    opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.Total,
                    opt => opt.MapFrom(o => Convert.ToDecimal(o.Total, new CultureInfo("es-CO"))));

            CreateMap<DetalleVenta, ReporteVentaViewModel>()
                .ForMember(
                    d => d.FechaRegistro,
                    opt => opt.MapFrom(o => o.IdVentaNavigation!.FechaRegistro!.Value.ToString("dd/MM/yyyy")))
                .ForMember(
                    d => d.NumeroVenta,
                    opt => opt.MapFrom(o => o.IdVentaNavigation!.NumeroVenta))
                .ForMember(
                    d => d.TipoDocumento,
                    opt => opt.MapFrom(o => o.IdVentaNavigation!.IdTipoDocumentoVentaNavigation!.Descripcion))
                .ForMember(
                    d => d.DocumentoCliente,
                    opt => opt.MapFrom(o => o.IdVentaNavigation!.DocumentoCliente))
                .ForMember(
                    d => d.NombreCliente,
                    opt => opt.MapFrom(o => o.IdVentaNavigation!.NombreCliente))
                .ForMember(
                    d => d.SubTotalVenta,
                    opt => opt.MapFrom(o => Convert.ToString(o.IdVentaNavigation!.SubTotal!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.ImpuestoTotalVenta,
                    opt => opt.MapFrom(o => Convert.ToString(o.IdVentaNavigation!.ImpuestoTotal!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.TotalVenta,
                    opt => opt.MapFrom(o => Convert.ToString(o.IdVentaNavigation!.Total!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.Producto,
                    opt => opt.MapFrom(o => o.DescripcionProducto))
                .ForMember(
                    d => d.Precio,
                    opt => opt.MapFrom(o => Convert.ToString(o.Precio!.Value, new CultureInfo("es-CO"))))
                .ForMember(
                    d => d.Total,
                    opt => opt.MapFrom(o => Convert.ToString(o.Total!.Value, new CultureInfo("es-CO"))));
            #endregion DetalleVenta

            #region Menu
            CreateMap<Menu, MenuViewModel>()
                .ForMember(
                    d => d.SubMenus,
                    opt => opt.MapFrom(o => o.InverseIdMenuPadreNavigation));
            #endregion Menu
        }
    }
}
