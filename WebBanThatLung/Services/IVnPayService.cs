using WebBanThatLung.Repository;
using WebBanThatLung.ViewModels;

namespace WebBanThatLung.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl (HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
