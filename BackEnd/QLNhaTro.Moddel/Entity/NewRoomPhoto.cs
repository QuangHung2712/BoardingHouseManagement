using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class NewRoomPhoto : BaseEntity
    {
        public string Path { get; set; }
        public long NewId { get; set; }
        public SharedResidents New { get; set; }
    }
}
