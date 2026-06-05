using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Persistence
{
    public class SpecificationsEvaluator
    {
        // Create Query - Build Query
        public static IQueryable<TEntity> CreateQuery<TEntity, Tkey>(IQueryable<TEntity> EntryPoint,
              ISpecifications<TEntity, Tkey> specifications) where TEntity : BaseEntity<Tkey>
        {
            var Query = EntryPoint; // _dbcontext.products

            if (specifications is not null)
            {

                if (specifications.Criteria is not null)
                {
                    Query = Query.Where(specifications.Criteria); //_dbcontext.products.Where()
                }

                if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    Query = specifications.IncludeExpressions.Aggregate(Query,
                        (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
                }

                if (specifications.OrderBy is not null)
                {
                    Query = Query.OrderBy(specifications.OrderBy);
                }

                if (specifications.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specifications.OrderByDescending);
                }

                if (specifications.IsPaginated)
                {
                    Query = Query.Skip(specifications.Skip).Take(specifications.Take);
                }
            }
            return Query;
        }

    }
}
