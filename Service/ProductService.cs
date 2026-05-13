using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Service.Abstractions;
using Service.Specifications;
using Shared;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOfWork unitOfWork ,IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();
            var mappedBrands =  mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return mappedBrands;

        }

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParams specs)
        {
           var products = await unitOfWork.GetRepository<Product,int>().GetAllAsync(new ProductWithFilterSpecification(specs));
            var CountSpecs = new ProductWithCountSpecification(specs);
            var totalItems = await unitOfWork.GetRepository<Product, int>().CountAsync(CountSpecs);
            var mappedProducts = mapper.Map<IEnumerable<ProductResultDto>>(products);
            return new PaginatedResult<ProductResultDto>(specs.PageIndex, specs.PageSize, totalItems, mappedProducts);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var mappedTypes = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return mappedTypes;
        }
         
        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(new ProductWithFilterSpecification(id));
            return product is null ? throw new ProductNotFoundException(id) : mapper.Map<ProductResultDto>(product);

        }
    }
}
