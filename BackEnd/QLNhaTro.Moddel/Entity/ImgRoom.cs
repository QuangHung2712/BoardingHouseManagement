using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class ImgRoom : BaseEntityIsDelete
    {
        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
        public string Path { get; set; }
    }
}
