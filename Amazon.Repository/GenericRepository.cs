using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Core.Specification;
using Amazon.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //return (IEnumerable<T>)await _context.Set<Product>().Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //=> await _context.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllwithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }

        public async Task CreateAsync(T entity)
            => await  _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
           => _context.Set<T>().Update(entity);
        //=> _context.Entry(entity).State = EntityState.Modified;


        public void Delete(T entity)
              => _context.Set<T>().Remove(entity);
    }
}
