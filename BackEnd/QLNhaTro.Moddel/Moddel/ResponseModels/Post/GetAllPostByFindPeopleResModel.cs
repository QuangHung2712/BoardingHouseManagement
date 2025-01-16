using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels.Post
{
    public class GetAllPostByFindPeopleResModel : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string Gender { get; set; }
        public bool IsSave { get; set; }
        public List<string> Img { get; set; }
    }
}
