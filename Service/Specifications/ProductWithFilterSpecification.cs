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
    public class ProductWithFilterSpecification : Specification<Product>
    {
        public ProductWithFilterSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

        }

        public ProductWithFilterSpecification(ProductSpecificationParams specs) :
            base(product => (!specs.BrandId.HasValue || specs.BrandId == product.BrandId) &&
            (!specs.TypeId.HasValue || specs.TypeId == product.TypeId) &&
            (string.IsNullOrEmpty(specs.Search) || product.Name.ToLower().Contains(specs.Search.ToLower().Trim())))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            ApplyPagination(specs.PageIndex, specs.PageSize);
            if (specs.Sort is not null)
            {
                switch (specs.Sort)
                {
                    case SortingOptions.PriceAsc:
                        AddOrderBy(product => product.Price);
                        break;
                    case SortingOptions.PriceDesc:
                        AddOrderByDescending(product => product.Price);
                        break;
                    case SortingOptions.NameAsc:
                        AddOrderBy(product => product.Name);
                        break;
                    case SortingOptions.NameDesc:
                        AddOrderByDescending(product => product.Name);
                        break;
                    default:
                        AddOrderBy(product => product.Name);
                        break;
                }

            }
        }
    }
}
