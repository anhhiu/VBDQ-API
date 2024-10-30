using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class TransactionDetailService : ITransactionDetailService
    {
        
        private readonly MyDbcontext context;

        public TransactionDetailService(MyDbcontext context)
        {
            this.context = context;
        }
        public async Task<(IEnumerable<TransactionDetail>, Mess)> GetAllTransactionDetail()
        {
            var transactionDetail = await context.TransactionDetails.Include(p => p.Product).Include(t => t.Transaction).ToListAsync();

            if (transactionDetail == null)
            {
                return (null!, new Mess { Error = "khong co", Status = "khong co gi" });
            }
            else
            {
                return (transactionDetail, new Mess {Error = null!,Status = "sucess" });
            }
        }
    }
}
