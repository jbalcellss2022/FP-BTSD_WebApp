using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository<T> : IProductRepository<T> where T : class
    {

        private readonly DbContext dbcontext;
        private readonly DbSet<T> dbSet;

        public ProductRepository(DbContext _dbContext)
        {
            dbcontext = _dbContext;
            dbSet = dbcontext.Set<T>();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Update(T entity) {
            dbSet.Attach(entity);
            dbcontext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity) {
            dbSet.Remove(entity); 
        }
    }
}
