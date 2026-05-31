using E_Commerce.Domain.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    internal class ProductWithTypeAndBrandSpecifications:BaseSpecifications<Product,int>
    {
        public ProductWithTypeAndBrandSpecifications():base()
        {
            AddInclude(p=>p.ProductType);
            AddInclude(p=>p.ProductBrand);
        }
       
    }
}
