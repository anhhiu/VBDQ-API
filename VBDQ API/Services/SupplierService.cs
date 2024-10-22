using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class SupplierService : ISupplierService
    {
        public Task<(SupplierDto, Mess)> AddSupplier(SupplierDto supplierDto)
        {
            throw new NotImplementedException();
        }

        public Task<Mess> DeleteSupplier(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<SupplierDto>, Mess)> GetAllSupplier()
        {
            throw new NotImplementedException();
        }

        public Task<(SupplierDto, Mess)> GetSupplierById()
        {
            throw new NotImplementedException();
        }

        public Task<(SupplierDto, Mess)> UpdatedSupplier(SupplierDto supplierDto, int id)
        {
            throw new NotImplementedException();
        }
    }
}
