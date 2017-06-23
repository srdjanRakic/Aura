using System;
using System.Linq;
using Aura.Core.Interfaces;
using Aura.Core.Models;
using Aura.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aura.Web.Api
{
    [Route("api/users")]
    [ValidateModel]
    public class UsersController : Controller
    {
        private readonly IUser _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUser user, ILogger<UsersController> logger)
        {
            _userService = user;
            _logger = logger;
        }

        // GET: api/users
        /// <summary>
        /// GET all users
        /// </summary>
        /// <returns>All the users</returns>
        [HttpGet]
        public IActionResult GetUsers()
        {
            var items = _userService.GetUsers().Select(UserDto.FromUser);

            return Ok(items);
        }

        // GET: api/users
        /// <summary>
        /// GET requested user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The requested user</returns>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = UserDto.FromUser(_userService.GetUserById(id));

                if (user != null) return Ok(user);

                _logger.LogInformation($"User with id {id} was not found.");

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting user with id {id}", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        // POST: api/users
        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Returns the created user</returns>
        [HttpPost("")]
        public IActionResult CreateUser([FromBody] UserCreateDto userDto)
        {
            if (userDto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = _userService.CreateUser(userDto);

                return Ok(UserDto.FromUser(user));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while creating user", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns>Returns the updated user</returns>
        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            if (userDto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = _userService.UpdateUser(id, userDto);

                if (user != null) return Ok(user);

                _logger.LogInformation($"User with id {id} was not found.");

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting user with id {id}", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);

                _logger.LogInformation($"User with id {id} was deleted.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting user with id {id}", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
    }
}
