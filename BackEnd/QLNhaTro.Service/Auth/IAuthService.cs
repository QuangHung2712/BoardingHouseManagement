using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service
{
    public interface IAuthService
    {
        string GenerateTokeLandlord();
        string GenerateTokeCustomer();
    }
}
