﻿using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository<T> : IProductRepository<T> where T : class
    {
        private readonly DbContext dbcontext;
        private readonly DbSet<T> dbSet;

        // Constructor & Dependency Injection //

        public ProductRepository(DbContext _dbContext)
        {
            dbcontext = _dbContext;
            dbSet = dbcontext.Set<T>();
        }

        // Implementation of repository methods //

        public T GetById(int id)
        {
            return dbSet.Find(id) ?? throw new Exception($"Entity with id {id} was not found.");
        }

        public IEnumerable<T> GetAll()
        {
            return [.. dbSet];
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