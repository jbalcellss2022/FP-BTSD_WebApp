namespace DataAccessLayer.Contracts
{
    public interface IProductRepository<T>
    {
        /// <summary>
        /// Locate a product in the database by its product id reference.
        /// </summary>
        /// <param name="id">The id code of the product you want to obtain. </param>
        /// <returns>An appProduct object that has been located. Otherwise null.</returns>

        T GetById(int id);

        IEnumerable<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
