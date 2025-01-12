using Microsoft.AspNetCore.Http;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.Post
{
    public interface IPostService
    {
        GetDetailPostResModel GetDetail(long Id);
        List<GetAllPostByCustomerResModel> GetByCustomer(long customerId);
        Task CreateEdit(CreateEditPostReqModel input, List<IFormFile> ImgRoom);
        Task DetelePost(long Id);
    }
}
