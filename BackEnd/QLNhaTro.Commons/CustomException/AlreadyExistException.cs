using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonConstants;

namespace QLNhaTro.Commons.CustomException
{
    public class AlreadyExistException : Exception
    {
        public string Name { get; private set; }
        public AlreadyExistException(string name)
        {
            Name = name;
        }
        public override string Message => string.Format(ExceptionMessage.ALREADY_EXIST, Name);
    }
}
