using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VBDQ_API.Conmon;
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
                if (customerPp == null)
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

            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
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

                return (customerClient, new Mess { Error = null, Status = "Sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(IEnumerable<Customer>, Mess)> GetAllCustomerByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, new Mess { Error = string.Empty, Status = "ten khong hop le" });
            }

            var customers = await context.Customers
                        .Where(c => c.CustomerName.ToLower().Contains(name.ToLower()))
                        .ToListAsync();

            if (customers == null)
            {
                return (null, new Mess { Error = "khong co", Status = "khong co khach hang nao co ten nhu vay" });

            }

            return (customers, new Mess { Error = null, Status = "sucess" });
        }

        public async Task<ServiceResponse<dynamic>> GetAllCustomerPT(int skip, int limit)
        {
            var response = new ServiceResponse<dynamic>();
            try
            {
                var query = context.Customers.AsNoTracking().OrderByDescending(c => c.CustomerId).AsQueryable();
                var (total, pNumber, pSize) = (0, 0, 0);
                total = await query.CountAsync();
                if (skip > 0 && limit > 0)
                {
                    pNumber = skip;
                    pSize = limit;
                    query = query.Skip((skip - 1) * limit).Take(limit);
                }
                var customer = await query.ToListAsync();

                if (customer == null)
                {
                    response.Data = new { };
                    response.Message = "khong co gi";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return response;
                }

                var resualt = new
                {
                    customer,
                    total,
                    skip,
                    limit,
                };

                response.Data = resualt;
                response.Message = "sucess";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;

            }
            catch (Exception ex)
            {
                response.Data = new { };
                response.Message = "error: " + ex.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return response;
            }
        }

        public async Task<(CustomerDto, Mess)> GetCustomerById(int id)
        {
            var customer = await context.Customers
                .Include(c => c.Transactions)
                .ThenInclude(C => C.TransactionDetails)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return (null, new Mess { Error = "loi roi", Status = $"Không có Id  = {id} dau cụ" });

            var customerclient = mapper.Map<CustomerDto>(customer);

            await context.SaveChangesAsync();

            return (customerclient, new Mess { Error = null, Status = "Xóa thành công khách hàng có Id = 7" });
        }

        public async Task<(Customer?, Mess)> GetcustomerByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (null, new Mess { Error = "ten khong hop le", Status = "ten khong hop le" });
            }

            //var customer = await context.Customers
            //    .FirstOrDefaultAsync(c => c.CustomerName.ToLower().Contains(name.ToLower()));

            // Tách tên thành các từ để tìm kiếm
            var nameParts = name.ToLower().Split(' '); // Chia tên thành từng từ

            var customer = await context.Customers
                .Where(c => nameParts.All(part => c.CustomerName.ToLower().Contains(part))) // Tìm kiếm tất cả các từ
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                return (customer, new Mess { Error = "khong tim thay", Status = "khong co khac hang nao co ten nhu the" });
            }
            return (customer, new Mess { Error = null, Status = "sucess" });
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
