using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPIProject.Interface;
using WebAPIProject.Models;
using WebAPIProject.Service;

namespace WebAPIProject.services
{
    public class UsersServise : IUserFinderServise
    {
        private readonly string jsonFilePath = "./Json/Users.json";

        public IEnumerable<User> GetAllUsers()
        {

            return GeneralServise.ReadFromJsonFile(jsonFilePath, "user").Cast<User>();
        }

        public User Get(string password)
        {
            var users = GeneralServise.ReadFromJsonFile(jsonFilePath, "user").Cast<User>().ToList();
            var currentUser = users.Find(x => x.Password == password);
            return currentUser;
        }

        public ActionResult Post(User newUser)
        {
            GeneralServise.WriteToJsonFile(jsonFilePath, newUser, "user");
            return new OkObjectResult("User added successfully");
        }

        public IActionResult Put(string password, User userToUpdate)
        {

            var deleteResult = Delete(password);
            if (deleteResult is ObjectResult obj && obj.StatusCode == 400)
            {
                return new ObjectResult("User ID not found") { StatusCode = 400 };
            }
            return GeneralServise.WriteToJsonFile(jsonFilePath, userToUpdate, "user");

        }

        public ActionResult Delete(string password)
        {
            var users = GeneralServise.ReadFromJsonFile(jsonFilePath, "user").Cast<User>().ToList();
            var userToDelete = users.Find(x => x.Password == password);
            if (userToDelete == null)
            {
                return new ObjectResult("User ID not found in token") { StatusCode = 400 };
            }
            users.Remove(userToDelete);
            string userJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(jsonFilePath, userJson);
            return new ObjectResult("User deleted successfully") { StatusCode = 201 };
        }
    }
    public static class UsersServiceHelper
    {
        public static void AddUsersService(this IServiceCollection services)
        {
            services.AddSingleton<IUserFinderServise, UsersServise>();
        }
    }

}