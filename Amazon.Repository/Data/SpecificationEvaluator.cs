using Amazon.Core.Entities;
using Amazon.Core.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository.Data
{
    internal class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> Spec )
        {
            var query = inputQuery; 

            if ( Spec.Criteria != null )
                query = query.Where( Spec.Criteria ); 

            if (Spec.OrderBy != null )
                query = query.OrderBy( Spec.OrderBy );

            if (Spec.OrderByDesending != null)
                query = query.OrderByDescending(Spec.OrderByDesending);

            if (Spec.IsPaginationEnable)
                query = query.Skip(Spec.Skip).Take(Spec.Take);

            query = Spec.Includes.Aggregate(query , (currentQuery, include) => currentQuery.Include(include));

            return query;
        }

    }
}
