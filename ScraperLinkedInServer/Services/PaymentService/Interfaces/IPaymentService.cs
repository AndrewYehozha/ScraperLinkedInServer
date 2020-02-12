using ScraperLinkedInServer.Models.Entities;
using System;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.PaymentService.Interfaces
{
    public interface IPaymentService
    {

        Task<PaymentViewModel> GetPaymentByGuideAsync(Guid guid);

        Task<PaymentViewModel> GetPaymentByAccountIdAsync(int accountId);

        Task<PaymentViewModel> InsertPaymentAsync(PaymentViewModel paymentVM);

        Task<PaymentViewModel> UpdatePaymentAsync(PaymentViewModel paymentVM);

        Task<PaymentViewModel> UpdatePaymentGuideAsync(int id, int accountId, Guid guid);
    }
}
