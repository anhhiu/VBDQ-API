﻿using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface ITransactionService
    {
        public Task<(IEnumerable<TransactionDto>, Mess)> GetAllTransaction();
        public Task< (TransactionDto, Mess)> AddTransaction(TransactionPP model);
        public Task<(TransactionDto, Mess)> UpdateTransaction(TransactionPP model, int id);
        public Task<(TransactionDto, Mess)> GetTransactionById(int id);
        public Task<Mess> DeleteTransaction(int id);

    }
}