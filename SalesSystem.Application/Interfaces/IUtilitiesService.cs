using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Application.Interfaces
{
    public interface IUtilitiesService
    {
        string GeneratePassword();
        string ConvertToSha256(string text);
    }
}
