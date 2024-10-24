﻿using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface ICustomerService
    {
        public Task<(IEnumerable<CustomerDto>, Mess)> GetAlLCustomer();
        public Task<(Customer, Mess)> AddCustomer(CustomerPP customerDto);
        public Task<(CustomerDto, Mess)> GetCustomerById(int id);
        public Task<(Customer, Mess)> UpdateCustomer(CustomerPP customerDto, int id);
        public Task<Mess> DeleteCustomer(int id);


    }
}
