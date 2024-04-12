namespace DataAccessLayer.Contracts
{
    public interface IProductRepository<T>
    {
        /// <summary>
        /// Returns a customer document summary object.
        /// </summary>
        /// <param name="request">A customer document request object that requires UserId(s) and the document Id being requested. </param>
        /// <returns>Response Code and document information.</returns>
        T GetById(int id);

        IEnumerable<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
