using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;

namespace WebAPIProject.Interface
{
    public interface IUserFinderServise
    {
        IEnumerable<User> GetAllUsers();
        User Get(string userId);
        ActionResult Post(User newUser);
        IActionResult Put(string password,User userToUpdate);
        ActionResult Delete(string userTd);        
    }
}
