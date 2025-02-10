using System.Reflection;
using FirstAPI.Data;
using FirstAPI.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

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

        public  async Task Update<T>(T entity) where T : class
        {
            // Get the Id property of the entity
            var keyProperty = typeof(T).GetProperty("Id");
            //if (keyProperty == null)
            //{
            //    throw new InvalidOperationException($"{typeof(T).Name} does not have an Id property.");
            //}

            // Get the value of the Id from the entity
            var id = keyProperty.GetValue(entity);
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Entity ID cannot be null.");
            }

            // Find the existing entity in the database by Id
            var existingEntity = await _context.Set<T>().FindAsync(id);
            if (existingEntity == null)
            {
                throw new EntityNotFoundException($"{typeof(T).Name} with ID {id} not found.");
            }
            Console.WriteLine($"cheese              {entity}, {existingEntity}");
            // Update only non-null properties (excluding the Id property)
             UpdateEntityProperties(existingEntity, entity);

            // Mark entity as modified (helps with EF tracking issues)
            _context.Entry(existingEntity).State = EntityState.Modified;

            // Save the changes asynchronously
            await _context.SaveChangesAsync();
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
            _context.SaveChanges();// saves changes to Database

        }

        // Update non-null, non-empty properties dynamically
        public void UpdateEntityProperties<T>(T existingEntity, T updatedEntity) where T : class
        {
            // Get type metadata of T
            Type entityType = typeof(T);

            foreach (PropertyInfo property in entityType.GetProperties())
            {
                // Skip read-only properties and the Id property
                if (!property.CanWrite || property.Name == "Id") continue;

                var existingValue = property.GetValue(existingEntity);
                var updatedValue = property.GetValue(updatedEntity);

                Console.WriteLine($"Checking property: {property.Name}");

                // If the updated value is not null, non-empty (for strings), and different, update it
                if (updatedValue != null &&
                    !(updatedValue is string str && string.IsNullOrWhiteSpace(str)) &&
                    !Equals(existingValue, updatedValue))
                {
                    property.SetValue(existingEntity, updatedValue);
                    Console.WriteLine($"Updated {property.Name} from {existingValue} to {updatedValue}");
                }
            }
            
        }
    }
}