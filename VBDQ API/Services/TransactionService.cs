using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly MyDbcontext context;
        private readonly IMapper mapper;

        public TransactionService(MyDbcontext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public Task<(TransactionDto, Mess)> AddTransaction(TransactionPP model)
        {
            throw new NotImplementedException();
        }

        public Task<Mess> DeleteTransaction(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<(IEnumerable<TransactionDto>, Mess)> GetAllTransaction()
        {
            var transaction = await context.Transactions
                .OrderByDescending(t => t.TransactionId)    
                .Include(t => t.TransactionDetails).ToListAsync();

            if (transaction == null)
                return (null, new Mess { Error = "loi roi", Status = "khong co ban ghi nao cả"});

            var transactionClient = mapper.Map<IEnumerable<TransactionDto>>(transaction);

            return (transactionClient, new Mess { Error = null, Status = "sucess" });
        }

        public Task<(TransactionDto, Mess)> GetTransactionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(TransactionDto, Mess)> UpdateTransaction(TransactionPP model, int id)
        {
            throw new NotImplementedException();
        }
    }
}
