using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces
{
    public interface IUserService 
    {
        void DeleteUser(int id);

        /// <summary>
        /// Return users by active state
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IEnumerable<User> FilterByActive(bool isActive);

        IEnumerable<User> GetAllUsers(); // Changed method name for consistency

        void UpdateUser(User updatedUser);

        void AddUser(User newUser);
        IEnumerable<object> GetAll();
    }
}