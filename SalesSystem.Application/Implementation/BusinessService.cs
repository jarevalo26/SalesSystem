using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Application.Implementation
{
    public class BusinessService : IBusinessService
    {
        private readonly IGenericRepository<Negocio> _repository;
        private readonly IFireBaseService _fireBase;

        public BusinessService(IGenericRepository<Negocio> repository, IFireBaseService fireBase)
        {
            _repository = repository;
            _fireBase = fireBase;
        }

        public async Task<Negocio> Get()
        {
            try
            {
                Negocio business = await _repository.Get(n => n.IdNegocio == 1);
                return business;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Negocio> SaveChanges(Negocio entity, Stream? logo = null, string logoName = "")
        {
            try
            {
                Negocio business = await _repository.Get(n => n.IdNegocio == 1);    
                business.NumeroDocumento = entity.NumeroDocumento;
                business.Nombre = entity.Nombre;
                business.Correo = entity.Correo;
                business.Direccion = entity.Direccion;
                business.Telefono = entity.Telefono;
                business.PorcentajeImpuesto = entity.PorcentajeImpuesto;
                business.SimboloMoneda = entity.SimboloMoneda;
                business.NombreLogo = business.NombreLogo == "" ? logoName : entity.NombreLogo;
                if (logo != null)
                {
                    string? logoUrl = await _fireBase.UploadStorage(logo, "carpeta_logo", business.NombreLogo!);
                    business.UrlLogo = logoUrl;
                }

                await _repository.Update(business);
                return business;
            }
            catch
            {
                throw;
            }
        }
    }
}
