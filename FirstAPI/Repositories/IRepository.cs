namespace FirstAPI.Repositories
{
    public interface IRepository
    {
        // Queryable method for flexible queries
        IQueryable<T> Query<T>() where T : class;

        // Get all records from the database
        List<T> GetAll<T>() where T : class;

        // Get a single record by ID
        T? GetById<T>(int id) where T : class;

        // Create a new entity
        void Create<T>(T entity) where T : class;

        // Update an existing entity
        void Update<T>(T entity) where T : class;

        // Delete an existing entity
        void Delete<T>(int id) where T : class;

        // Save changes to the database
        void SaveChanges();
    }
}
