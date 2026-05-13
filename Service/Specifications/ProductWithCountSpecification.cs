using Domain.Contracts;
using Domain.Entities;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductWithCountSpecification :Specification<Product>
    {
        public ProductWithCountSpecification(ProductSpecificationParams specs) :
           base(product => (!specs.BrandId.HasValue || specs.BrandId == product.BrandId) &&
           (!specs.TypeId.HasValue || specs.TypeId == product.TypeId) &&
           (string.IsNullOrEmpty(specs.Search) || product.Name.ToLower().Contains(specs.Search.ToLower().Trim())))
        {
        }
    }
}