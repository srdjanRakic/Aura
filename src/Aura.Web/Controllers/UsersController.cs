using System.Linq;
using Aura.Core.Interfaces;
using Aura.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aura.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUser _userService;

        public UsersController(IUser userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetUsers().Select(UserDto.FromUser);

            return View(users);
        }
    }
}
