using FirstAPI.Data;
using FirstAPI.Repositories.Exceptions;

namespace FirstAPI.Repositories
{
    public class Repository(ApplicationDBcontext _context) : IRepository
    {

        public T? GetById<T>(int id) where T : class
        {
            try
            {
                var entity = _context.Set<T>().Find(id);
                if (entity == null)
                {
                    throw new EntityNotFoundException($"{typeof(T).Name} with ID {id} not found.");
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"An error occurred while retrieving {typeof(T).Name}.", ex);
            }
        }

        public List<T> GetAll<T>() where T : class
        {
            try
            {
                return _context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"An error occurred while retrieving all {typeof(T).Name}s.", ex);
            }
        }

        public void Create<T>(T entity) where T : class
        {
            try
            {
                _context.Set<T>().Add(entity);
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"An error occurred while creating {typeof(T).Name}.", ex);
            }
        }

        public void Update<T>(T entity) where T : class
        {
            try
            {
                var existingEntity = _context.Set<T>().Find(entity);
                if (existingEntity == null)
                {
                    throw new EntityNotFoundException("Entity not found.");
                }

                // Use the method to update only non-null and non-empty properties
                UpdateNonNullProperties(existingEntity, entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("An error occurred while updating the entity.", ex);
            }
        }

        public void Delete<T>(int id) where T : class
        {
            try
            {
                var entity = _context.Set<T>().Find(id);
                if (entity == null)
                {
                    throw new EntityNotFoundException($"{typeof(T).Name} with ID {id} not found.");
                }

                _context.Set<T>().Remove(entity);
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"An error occurred while deleting {typeof(T).Name}.", ex);
            }
        }

        public IQueryable<T> Query<T>() where T : class
        {
            try
            {
                return _context.Set<T>();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"An error occurred while querying {typeof(T).Name}.", ex);
            }
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("An error occurred while saving changes to the database.", ex);
            }
        }

        // Update non-null, non-empty properties dynamically
        public static void UpdateNonNullProperties<T>(T existingEntity, T updatedEntity) where T : class
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                // Skip read-only properties and the Id property
                if (!property.CanWrite || property.Name == "Id") continue;

                var newValue = property.GetValue(updatedEntity);
                var existingValue = property.GetValue(existingEntity);

                // Check if the new value is not null, not empty (for strings), and is different from the existing value
                if (newValue != null &&
                    !(newValue is string str && string.IsNullOrWhiteSpace(str)) &&
                    !Equals(newValue, existingValue))
                {
                    property.SetValue(existingEntity, newValue);
                }
            }

        }
    }
}