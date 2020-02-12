using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.PaymentRepository.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.PaymentRepository
{
    public class PaymentRepository : IPaymentRepository
    {
        public async Task<Payment> GetPaymentByGuideAsync(Guid guid)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Payments.Where(x => x.Guide == guid).FirstOrDefaultAsync();
            }
        }

        public async Task<Payment> GetPaymentByAccountIdAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Payments.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
            }
        }

        public async Task<Payment> InsertPaymentAsync(Payment payment)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                payment.Guide = Guid.NewGuid();
                payment.CreateOn = DateTime.UtcNow;
                payment.PaymentOn = DateTime.UtcNow;
                payment.UpdateOn = DateTime.UtcNow;

                var paymentDb = db.Payments.Add(payment);
                await db.SaveChangesAsync();

                return paymentDb;
            }
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var paymentDb = await db.Payments.Where(x => x.Id == payment.Id).FirstOrDefaultAsync();

                var totalDays = (DateTime.UtcNow - paymentDb.PaymentOn.Value).TotalDays;

                if (totalDays > paymentDb.Validity)
                {
                    paymentDb.Validity = payment.Validity;
                }
                else if (totalDays == paymentDb.Validity)
                {
                    paymentDb.Validity = payment.Validity + 1;
                }
                else if (totalDays < paymentDb.Validity)
                {
                    paymentDb.Validity = paymentDb.Validity - (int)totalDays + payment.Validity + 1;
                }

                paymentDb.Guide = Guid.NewGuid();
                paymentDb.PaymentOn = DateTime.UtcNow;
                paymentDb.UpdateOn = DateTime.UtcNow;

                await db.SaveChangesAsync();

                return paymentDb;
            }
        }

        public async Task<Payment> UpdatePaymentGuideAsync(int id, int accountId, Guid guid)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var paymentDb = await db.Payments.Where(x => x.Id == id && x.AccountId == accountId).FirstOrDefaultAsync();

                paymentDb.Guide = guid;
                paymentDb.UpdateOn = DateTime.UtcNow;

                await db.SaveChangesAsync();

                return paymentDb;
            }
        }
    }
}