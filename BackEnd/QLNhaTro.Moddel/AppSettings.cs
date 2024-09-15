using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel
{
    public class AppSettings
    {
        public Jwt Jwt { get; set; }
    }
    public class Jwt
    {
        public string SecretKey { get; set; }
    }
}
