using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;
using WebAPIProject.Services;
using WebAPIProject.Interface;



namespace WebApiProject.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserFinderServise UserFinderServise;
    public UsersController(IUserFinderServise UserFinderServise)
    {
        this.UserFinderServise = UserFinderServise;
    }

    [HttpGet("GetAllUsers")]
    [Authorize(Policy = "SuperAdmin")]
    public IActionResult GetAllUsers()
    {
        var users = UserFinderServise.GetAllUsers();
        if (users != null)
        {
            return Ok(users);
        }
        return StatusCode(500, "Data reading failed while retrieving users.");
    }

    [HttpGet]
    public IActionResult Get()
    {
        ObjectResult passObj = (ObjectResult)GeneralServise.GetUserPasswordFromToken(HttpContext);
        var password = passObj.Value as string;
        var currentUser = UserFinderServise.Get(password);
        if (currentUser == null)
        {
            return StatusCode(500, "Data reading failed while retrieving users.");
        }
        return Ok(currentUser);
    }

    [HttpPost]
    [Authorize(Policy = "SuperAdmin")]
    public IActionResult Post([FromBody] User newUser)
    {
        return (UserFinderServise.Post(newUser));
    }

    [HttpPut("{password}")]
    public IActionResult Put(string password,User userToUpdate)
    {
        ObjectResult typeObj = (ObjectResult)GeneralServise.GetUserTypeFromToken(HttpContext);
        var type = typeObj.Value as string;
        ObjectResult passObj = (ObjectResult)GeneralServise.GetUserPasswordFromToken(HttpContext);
        var currentPassword = passObj.Value as string;
        if (type.Equals("SuperAdmin") || password.Equals(currentPassword))
        {
            return UserFinderServise.Put(password,userToUpdate);
        }
        return Forbid("You do not have permission to update this user.");
    }

    [HttpDelete("{password}")]
    public IActionResult Delete(string password)
    {
        ObjectResult typeObj = (ObjectResult)GeneralServise.GetUserTypeFromToken(HttpContext);
        var type = typeObj.Value as string;
        ObjectResult passObj = (ObjectResult)GeneralServise.GetUserPasswordFromToken(HttpContext);
        var userPassword = passObj.Value as string;
        if (type.Equals("SuperAdmin") || userPassword.Equals(password))
        {
            return UserFinderServise.Delete(password);
        }
        return Forbid("You do not have permission to delete this user.");
    }

}