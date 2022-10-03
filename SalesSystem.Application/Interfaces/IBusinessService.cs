using SalesSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Application.Interfaces
{
    public interface IBusinessService
    {
        Task<Negocio> Get();
        Task<Negocio> SaveChanges(Negocio entity, Stream? logo = null, string logoName = "");
    }
}
