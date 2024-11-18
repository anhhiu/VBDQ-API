using VBDQ_API.Conmon;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface ITransactionService
    {
        public Task<(IEnumerable<TransactionDto>?, Mess)> GetAllTransaction();
        public Task<ServiceResponse<dynamic>?> UpdateTransactionAsync(TransactionUpdate model, int id);
        public Task<(TransactionDto?, Mess)> GetTransactionById(int id);
        public Task<Mess> DeleteTransaction(int id);
        Task<ServiceResponse<dynamic>?> AddTransactionAsync(TransactionCreate model);
        Task<(ServiceResponse<dynamic>?, int skip, int limit, int total)> GetTransactionAsync(GridQuery? gridQuery);

    }
}
