using AutoMapper;
using VBDQ_API.Dtos;
using VBDQ_API.Models;

namespace VBDQ_API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest =>dest.ProductDtos, opt => opt.MapFrom(src => src.Products));

            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.TransactionDtos, opt => opt.MapFrom(src => src.Transactions));
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.Transactions, opt => opt.Ignore());

            CreateMap<Supplier, SupplierDto>()
                .ForMember(dest => dest.ProductDtos, opt => opt.MapFrom(src => src.Products));

            CreateMap<SupplierDto, Supplier>()
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.TransactionDetailDtos, opt => opt.MapFrom(src => src.TransactionDetails));
               
            CreateMap<ProductDto, Product>()
                 .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                 .ForMember(dest => dest.TransactionDetails, opt => opt.Ignore());

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.TransactionDetails, opt => opt.MapFrom(src => src.TransactionDetails));
            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.TransactionId, opt => opt.Ignore());

            CreateMap<TransactionDetail, TransactionDetailDto>();

            CreateMap<TransactionDetailDto, TransactionDetail>()
               .ForMember(dest => dest.TransactionDetailId, opt => opt.Ignore());
               
        }
    }
}
