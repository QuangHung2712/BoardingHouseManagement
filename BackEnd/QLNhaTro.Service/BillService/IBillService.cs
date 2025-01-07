using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.BillService
{
    public interface IBillService
    {
        Task SubmitRequesInformation();
        GetInfoBillResModel GetInfoBill(string Id);
        Task CalculateInvoice(GetInfoBillResModel input);
        Task PayBill(long id);
        List<GetRequestPaymentConfirmationResModel> GetRequestPayment(long landlordId);
        Task RefusePay(RefusePayReqModel input);
        Task AcceptPayments(RefusePayReqModel input);
        List<GetAllBillByTowerResModel> GetAll(long towerId);
        Task DeleteBill(long billId);
        GetDetailBillResModel GetDetail(long billId);
        Task UpdateBill(UpdateBillReqModel input);
    }
}
