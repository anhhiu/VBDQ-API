using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface ITransactionDetailService
    {
        public Task<(IEnumerable<TransactionDetail>, Mess)> GetAllTransactionDetail();
    }
}
