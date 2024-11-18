using VBDQ_API.Conmon;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface ISupplierService
    {
        public Task<(IEnumerable<SupplierDto>, Mess)> GetAllSupplier();
        public Task<(SupplierDto, Mess)> GetSupplierById(int id);
        public Task<(SupplierDto, Mess)> AddSupplier(SupplierDto supplierDto);
        public Task<(SupplierDto, Mess)> UpdatedSupplier(SupplierDto supplierDto, int id);
        public Task<Mess> DeleteSupplier(int id);

        Task<ServiceResponse<dynamic>> GetAllSupplierAsync();
        Task<ServiceResponse<dynamic>> GetAllSupplierAsync2(int pageNumber, int pageSize);
        Task<ServiceResponse<dynamic>> CreateSupplierAsync(SuppllierCreate model);
        Task<ServiceResponse<dynamic>> UpdateSupplierAsync(SuppllierUpdate model, int id);
        Task<ServiceResponse<dynamic>> GetSupplierByIdAsync(int id);
        Task<ServiceResponse<dynamic>> DeleteSupplierByIdAsync(int id);

    }
}
