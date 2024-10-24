using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly MyDbcontext context;
        private readonly IMapper mapper;

        public SupplierService(MyDbcontext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<(SupplierDto, Mess)> AddSupplier(SupplierDto supplierDto)
        {
            try
            {
                var supplier = mapper.Map<Supplier>(supplierDto);

                await context.Suppliers.AddAsync(supplier);

                await context.SaveChangesAsync();

                var supplierclient = mapper.Map<SupplierDto>(supplier);

                return (supplierclient, new Mess { Error = null, Status = "thanh nha phan phoi thanh cong" });
            }
            catch (Exception ex) 
            {
                return (null, new Mess { Error = "500", Status = ex.Message});
            }
        }

        public async Task<Mess> DeleteSupplier(int id)
        {
            var supplier = await context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return new Mess { Error = "500", Status = "Error" };
            }

            context.Suppliers.Remove(supplier);
            await context.SaveChangesAsync();

            var supplierclient = mapper.Map<SupplierDto>(supplier);

            return ( new Mess {Error = null, Status = "xoa thanh cong" });
        }

        public async Task<(IEnumerable<SupplierDto>, Mess)> GetAllSupplier()
        {
            var supplier = await context.Suppliers.OrderByDescending(s => s.SupplierId).Include(s => s.Products).ToListAsync();

            var supplierdto = mapper.Map<IEnumerable<SupplierDto>>(supplier);

            return (supplierdto, new Mess { Error = null, Status = "sucess"});
        }

        public async Task<(SupplierDto, Mess)> GetSupplierById(int id)
        {
            var supplier = await context.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier == null)
            {
                return(null, new Mess { Error = "500", Status = "Error" });
            }
            var supplierdto = mapper.Map<SupplierDto>(supplier);
            return (supplierdto, new Mess {Error = null, Status = "sucess" });
        }

        public async Task<(SupplierDto, Mess)> UpdatedSupplier(SupplierDto supplierDto, int id)
        {
            var supplier = await context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id);

            if (supplier == null)
            {
                return (null, new Mess { Error= "500", Status= "Error" });
            }

            mapper.Map(supplierDto, supplier);

            await context.SaveChangesAsync();

            var supplierclient = mapper.Map<SupplierDto>(supplier);

            return (supplierclient, new Mess { Error = null, Status = "sucess" });

            
        }
    }
}
