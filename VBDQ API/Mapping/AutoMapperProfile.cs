using AutoMapper;
using VBDQ_API.Dtos;
using VBDQ_API.Models;

namespace VBDQ_API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap().ForMember(dest => dest.CategoryId, opt => opt.Ignore());
            CreateMap<Customer, CustomerDto>().ReverseMap().ForMember(dest => dest.CustomerId, opt => opt.Ignore());
            CreateMap<Supplier, SupplierDto>().ReverseMap().ForMember(dest => dest.SupplierId, opt => opt.Ignore());
            CreateMap<Product, ProductDto>().ReverseMap().ForMember(dest => dest.ProductId, opt => opt.Ignore());
            CreateMap<Transaction, TransactionDto>().ReverseMap().ForMember(dest => dest.TransactionId, opt => opt.Ignore());
            CreateMap<TransactionDetail, TransactionDetailDto>().ReverseMap().ForMember(dest => dest.TransactionDetailId, opt => opt.Ignore());
        }
    }
}
