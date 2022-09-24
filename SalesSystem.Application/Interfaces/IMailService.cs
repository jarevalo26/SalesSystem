using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Application.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendEmail(string mailFor, string mailSubject, string message);
    }
}
