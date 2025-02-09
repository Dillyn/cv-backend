using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Services.Exceptions;

namespace FirstAPI.Services
{
    public class UserService(IRepository _repository)
    {


        public User GetUserById(int id)
        {
            try
            {
                var user = _repository.GetById<User>(id);
                if (user == null)
                {
                    throw new UserNotFoundException($"User with ID {id} not found.");
                }
                return user;
            }
            catch (UserNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving the user.", ex);
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return _repository.GetAll<User>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while retrieving all users.", ex);
            }
        }

        public void CreateUser(User user)
        {
            try
            {
                _repository.Create(user);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while creating the user.", ex);
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                _repository.Update(user);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while updating the user.", ex);
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                _repository.Delete<User>(id);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An error occurred while deleting the user.", ex);
            }
        }

        public List<User> FindUsersByFirstLetter(char letter)
        {
            if (!char.IsLetter(letter))
            {
                throw new ArgumentException("Invalid character. Please provide a letter.");
            }

            string firstLetter = letter.ToString().ToLower();



            // Filter users by the first name starting with the given letter and retrieves them from the Database
            var filteredUsers = _repository
                .Query<User>()                                    // returns IQueryable<User>
                .Where(u => !string.IsNullOrEmpty(u.Name) &&      // chacks if string is not null or emty  
                     u.Name.ToLower().StartsWith(firstLetter))              // and checks if name start with firstletter
                        .ToList();



            //If no users are found, throw an exception
            //if (filteredUsers.Count == 0 || !filteredUsers)
            //{
            //    throw new Exception($"No users have been found with a name that begins with the letter '{letter}'.");
            //}

            return filteredUsers;
        }
    }
}
