using Amazon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; set; } 
        public Expression<Func<T, object>> OrderByDesending { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnable { get; set; }

        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            this.Criteria = criteria;
        }

        public void AddIncludes(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }

        public void AddOrderBy (Expression<Func<T, object>> orderby)
        {
            OrderBy = orderby;
        }

        public void AddOrderByDesending(Expression<Func<T, object>> orderbyDese)
        {
            OrderByDesending = orderbyDese;
        }

        public void ApplyPagintion(int skip , int take)
        {
            IsPaginationEnable = true;
            Skip = skip;
            Take = take;
        }

    }
}
