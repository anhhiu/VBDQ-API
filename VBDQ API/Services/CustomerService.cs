using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;


namespace VBDQ_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly MyDbcontext context;
        private readonly IMapper mapper;

        public CustomerService(MyDbcontext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<(Customer, Mess)> AddCustomer(CustomerPP customerPp)
        {
            try
            {
                if(customerPp == null)
                    return (null, new Mess { Error = "loi roi", Status = "Đầu vào không hợp lệ" });

                var customer = new Customer()
                {
                    CustomerName = customerPp.CustomerName,
                    Address = customerPp.Address,
                    Phone = customerPp.Phone,
                };

                await context.Customers.AddAsync(customer);

                await context.SaveChangesAsync();

                return (customer, new Mess { Error = null, Status = "success" });

            }catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message});
            }

            
        }

        public async Task<Mess> DeleteCustomer(int id)
        {
            var customer = await context.Customers.FindAsync(id);

            if (customer == null)
                return (new Mess { Error = "loi roi", Status = $"Không có Id  = {id} dau cụ" });
            context.Customers.Remove(customer);

            await context.SaveChangesAsync();

            return new Mess { Error = null, Status = "Xóa thành công khách hàng có Id = 7" };
        }

        public async Task<(IEnumerable<CustomerDto>, Mess)> GetAlLCustomer()
        {
            try
            {
               var customer = await context.Customers
                    .Include(c => c.Transactions)
                    .ThenInclude(C => C.TransactionDetails)
                    .OrderByDescending(c => c.CustomerId)
                    .ToListAsync();

                var customerClient = mapper.Map<IEnumerable<CustomerDto>>(customer);

                return (customerClient, new Mess {Error = null, Status = "Sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(CustomerDto, Mess)> GetCustomerById(int id)
        {
            var customer = await context.Customers
                .Include(c => c.Transactions)
                .ThenInclude (C => C.TransactionDetails)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return (null, new Mess { Error = "loi roi", Status = $"Không có Id  = {id} dau cụ" });

            var customerclient = mapper.Map<CustomerDto>(customer);

            await context.SaveChangesAsync();

            return (customerclient, new Mess { Error = null, Status = "Xóa thành công khách hàng có Id = 7" });
        }

        public async Task<(Customer, Mess)> UpdateCustomer(CustomerPP customerDto, int id)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return (null, new Mess { Error = "loi roi", Status = $"Không có Id  = {id} dau cụ" });

            customer.CustomerName = customerDto.CustomerName;
            customer.Address = customerDto.Address;
            customer.Phone = customerDto.Phone;

            await context.SaveChangesAsync();

            return (customer, new Mess { Error = null, Status = "update customer sucess" });
        }
    }
}
