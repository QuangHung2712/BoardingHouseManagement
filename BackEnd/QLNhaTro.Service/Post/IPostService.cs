using Microsoft.AspNetCore.Http;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Moddel.Moddel.ResponseModels.Post;
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
        Task Found(long postId);
        Task Repost(long postId);
        List<GetAllPostByFindPeopleResModel> FindPeople(string address, decimal priceForm, decimal priceArrive, long customerId, int genter);
        Task SavePost(long customerId, long postId, bool status);
        GetDetailPostByFindResModel GetDetailPostByFind(long Id, long customerId);
    }
}
