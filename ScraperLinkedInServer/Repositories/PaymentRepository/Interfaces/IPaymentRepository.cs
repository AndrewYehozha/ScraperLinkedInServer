using ScraperLinkedInServer.Database;
using System;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.PaymentRepository.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentByGuideAsync(Guid guid);

        Task<Payment> GetPaymentByAccountIdAsync(int accountId);

        Task<Payment> InsertPaymentAsync(Payment payment);

        Task<Payment> UpdatePaymentAsync(Payment payment);

        Task<Payment> UpdatePaymentGuideAsync(int id, int accountId, Guid guid);
    }
}
