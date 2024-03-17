using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;
 public void AddUser(User user) => _dataAccess.Create(user);

    public void DeleteUser(int id)
    {
        var userToDelete = _dataAccess.GetAll<User>().Where(x => x.Id == id).First();

        if (userToDelete == null)
        {
            throw new ArgumentException("User not found");
        }

        _dataAccess.Delete(userToDelete);
    }

    public IEnumerable<User> FilterByActive(bool isActive)
    {
        return _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive).ToList();
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public void UpdateUser(User user)
    {
        var userToEdit = _dataAccess.GetAll<User>().Where(x => x.Id == user.Id).First();

        if (userToEdit == null)
        {
            throw new InvalidOperationException("User not found in the data store");
        }

        userToEdit.Forename = user.Forename;
        userToEdit.Surname = user.Surname;
        userToEdit.Email = user.Email;

        _dataAccess.Update(userToEdit);
    }
}