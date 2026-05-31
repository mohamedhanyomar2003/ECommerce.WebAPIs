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
        public static IQueryable<TEntity> CreateQuery<TEntity, Tkey>(IQueryable<TEntity> EntryPoint,
              ISpecifications<TEntity, Tkey> specifications) where TEntity : BaseEntity<Tkey>
        {
            var Query = EntryPoint;

            if (specifications is not null)
            {
                if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    Query = specifications.IncludeExpressions.Aggregate(Query, 
                        (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
                }
            }
            return Query;
        }
        
    }
}
