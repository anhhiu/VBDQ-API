using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface ISupplierService
    {
        public Task<(IEnumerable<SupplierDto>, Mess)> GetAllSupplier();
        public Task<(SupplierDto, Mess)> GetSupplierById();
        public Task<(SupplierDto, Mess)> AddSupplier(SupplierDto supplierDto);
        public Task<(SupplierDto, Mess)> UpdatedSupplier(SupplierDto supplierDto, int id);
        public Task<Mess> DeleteSupplier(int id);
    }
}
