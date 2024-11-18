using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using System.Net;
using VBDQ_API.Conmon;
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

        public async Task<ServiceResponse<dynamic>> CreateSupplierAsync(SuppllierCreate model)
        {
            var response = new ServiceResponse<dynamic>();

            if(model == null)
            {
                response.Data = new { };
                response.Message = "input error";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var supplier = new Supplier
            {
                SupplierName = model.SupplierName,
                ContactName = model.ContactName,
                Phone = model.Phone,
                Address = model.Address,
                Email = model.Email,
            };

            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();

            response.Data = supplier;
            response.Message = "ok";
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;

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

        public async Task<ServiceResponse<dynamic>> DeleteSupplierByIdAsync(int id)
        {
            var respone = new ServiceResponse<dynamic>();
            var supplier = await context.Suppliers.FirstOrDefaultAsync(p => p.SupplierId == id);
            
            if(supplier == null)
            {
                respone.Data = new { };
                respone.Message = "khong tim thays NCC nao co id = " + id;
                respone.StatusCode = (int)HttpStatusCode.NotFound;
                return respone;
            }

            context.Suppliers.Remove(supplier);
            await context.SaveChangesAsync();

            respone.Data = supplier;
            respone.Message = " da xoa NCC co id: " + id + "thanh cong";
            respone.StatusCode = (int)HttpStatusCode.NoContent;
            return respone;
        }

        public async Task<(IEnumerable<SupplierDto>, Mess)> GetAllSupplier()
        {
            var supplier = await context.Suppliers.OrderByDescending(s => s.SupplierId).Include(s => s.Products).ToListAsync();

            var supplierdto = mapper.Map<IEnumerable<SupplierDto>>(supplier);

            return (supplierdto, new Mess { Error = null, Status = "sucess"});
        }

        public async Task<ServiceResponse<dynamic>> GetAllSupplierAsync()
        {
            var respone = new ServiceResponse<dynamic>();
            var supplier = await context.Suppliers.ToListAsync();

            if (supplier == null)
            {
                respone.Data = new { };
                respone.Message = "khong tim thays ";
                respone.StatusCode = (int)HttpStatusCode.NotFound;
                return respone;
            }

            respone.Data = supplier;
            respone.Message = "ok";
            respone.StatusCode = (int)HttpStatusCode.OK;
            return respone;
        }

        public async Task<ServiceResponse<dynamic>> GetAllSupplierAsync2(int pageNumber, int pageSize)
        {
            var respone = new ServiceResponse<dynamic>();
            var query = context.Suppliers.AsQueryable();
            var (total, skip, take) = (0, 0, 0);
            total = query.Count();
            if(pageNumber >0 && pageSize > 0)
            {
                skip = pageNumber;
                take = pageSize;
                query = query.Skip((pageNumber - 1) * pageSize).Take(take);

            }
            var supplier = query.ToListAsync();
            if (supplier == null)
            {
                respone.Data = new { };
                respone.Message = "khong tim thays ";
                respone.StatusCode = (int)HttpStatusCode.NotFound;
                return respone;
            }
            var result = new
            {
                Data = supplier,
                Total = total,
                Skip = skip,
                Take = take,
            };

            respone.Data = result;
            respone.Message = "ok";
            respone.StatusCode = (int)HttpStatusCode.OK;
            return respone;

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

        public async Task<ServiceResponse<dynamic>> GetSupplierByIdAsync(int id)
        {
            var respone = new ServiceResponse<dynamic>();
            var supplier = await context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                respone.Data = new { };
                respone.Message = "khong tim thays ";
                respone.StatusCode = (int)HttpStatusCode.NotFound;
                return respone;
            }

            respone.Data = supplier;
            respone.Message = "ok";
            respone.StatusCode = (int)HttpStatusCode.OK;
            return respone;
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

        public async Task<ServiceResponse<dynamic>> UpdateSupplierAsync(SuppllierUpdate model, int id)
        {
            var respone = new ServiceResponse<dynamic>();
            var supplier = await context.Suppliers.FirstOrDefaultAsync(t => t.SupplierId == id);

            if (supplier == null)
            {
                respone.Data = new { };
                respone.Message = "khong tim thays ";
                respone.StatusCode = (int)HttpStatusCode.NotFound;
                return respone;
            }

            supplier.SupplierName = model.SupplierName;
            supplier.ContactName = model.ContactName;
            supplier.Address = model.Address;
            supplier.Phone = model.Phone;
            supplier.Email = model.Email;

            context.Suppliers.Update(supplier);
            await context.SaveChangesAsync();

            respone.Data = supplier;
            respone.Message = "ok";
            respone.StatusCode = (int)HttpStatusCode.OK;
            return respone;
        }
    }
}
