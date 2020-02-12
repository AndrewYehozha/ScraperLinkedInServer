using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.PaymentRepository.Interfaces;
using ScraperLinkedInServer.Services.PaymentService.Interfaces;
using System;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentViewModel> GetPaymentByAccountIdAsync(int accountId)
        {
            var paymentDb = await _paymentRepository.GetPaymentByAccountIdAsync(accountId);
            return Mapper.Instance.Map<Payment, PaymentViewModel>(paymentDb);
        }

        public async Task<PaymentViewModel> GetPaymentByGuideAsync(Guid guid)
        {
            var paymentDb = await _paymentRepository.GetPaymentByGuideAsync(guid);
            return Mapper.Instance.Map<Payment, PaymentViewModel>(paymentDb);
        }

        public async Task<PaymentViewModel> InsertPaymentAsync(PaymentViewModel paymentVM)
        {
            var paymentDb = Mapper.Instance.Map<PaymentViewModel, Payment>(paymentVM);
            paymentDb = await _paymentRepository.InsertPaymentAsync(paymentDb);
            return Mapper.Instance.Map<Payment, PaymentViewModel>(paymentDb);
        }

        public async Task<PaymentViewModel> UpdatePaymentAsync(PaymentViewModel paymentVM)
        {
            var paymentDb = Mapper.Instance.Map<PaymentViewModel, Payment>(paymentVM);
            paymentDb = await _paymentRepository.UpdatePaymentAsync(paymentDb);
            return Mapper.Instance.Map<Payment, PaymentViewModel>(paymentDb);
        }

        public async Task<PaymentViewModel> UpdatePaymentGuideAsync(int id, int accountId, Guid guid)
        {
            var paymentDb = await _paymentRepository.UpdatePaymentGuideAsync(id, accountId, guid);
            return Mapper.Instance.Map<Payment, PaymentViewModel>(paymentDb);
        }
    }
}