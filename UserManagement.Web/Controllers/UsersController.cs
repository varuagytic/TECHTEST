using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(bool? isActive = null)
    {
        IEnumerable<User> users;

        if (isActive.HasValue)
        {
            users = _userService.FilterByActive(isActive.Value);
        }
        else
        {
            users = _userService.GetAll();
        }

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        }).ToList();

        var model = new UserListViewModel
        {
            Items = items
        };

        return View(model);
    }

    [HttpGet]
    [Route("view")]
    public ViewResult View(int id)
    {
        var user = _userService.GetAll().First(p => p.Id == id);

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpGet]
    [Route("edit")]
    public IActionResult Edit(int id)
    {
        var user = _userService.GetAll().First(p => p.Id == id);

        if (user == null)
        {
            return NotFound(); 
        }

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpGet]
    [Route("add")]
    public IActionResult Add()
    {
        var model = new AddUserViewModel();

        return View(model);
    }

    [HttpPost]
    [Route("add")]
    public IActionResult Add(AddUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var newUser = new User
        {
            Forename = model.Forename ?? string.Empty,
            Surname = model.Surname ?? string.Empty,
            Email = model.Email ?? string.Empty,
            DateOfBirth = model.DateOfBirth 
        };

        _userService.AddUser(newUser);

        return RedirectToAction("List");
    }

    [HttpPost]
    [Route("edit")]
    public IActionResult Edit(int id, EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _userService.GetAll().First(p => p.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        user.Forename = model.Forename ?? user.Forename;
        user.Surname = model.Surname ?? user.Surname;
        user.Email = model.Email ?? user.Email;
        user.DateOfBirth = model.DateOfBirth ?? user.DateOfBirth;
        user.IsActive = model.IsActive;

        _userService.UpdateUser(user);

        return RedirectToAction("List");
    }

    [HttpGet]
    [Route("delete")]
    public IActionResult Delete(int id)
    {
        var user = _userService.GetAll().First(p => p.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var model = new DeleteUserViewModel
        {
            UserId = user.Id,
            Name = $"{user.Forename} {user.Surname}"
        };

        return View(model);
    }

    [HttpPost]
    [Route("delete")]
    public IActionResult PostDelete(int id)
    {
        _userService.DeleteUser(id); 
        return RedirectToAction("List"); 
    }

}