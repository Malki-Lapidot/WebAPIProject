using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Interface;
using WebAPIProject.Models;
using WebAPIProject.Services;

namespace WebAPIProject.Controllers;
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly string jsonFilePath = "./Json/Users.json";
    private ITokenService TokenService;
    public LoginController(ITokenService tokenService)
    {
        this.TokenService = tokenService;
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult<String> Login([FromBody] User User)
    {
        var claims = new List<Claim>();
        var users = GeneralServise.ReadFromJsonFile(jsonFilePath, "user").Cast<User>().ToList();
        var currentUser = users.Find(x => x.Password == User.Password);

        if (currentUser == null || !currentUser.UserName.Equals(User.UserName))
        {
            return Unauthorized();
        }
        else
        {
            claims.Add(new Claim("type", currentUser.Permission));
        }

        claims.Add(new Claim("password", User.Password));
        claims.Add(new Claim("userName", User.UserName));
        var token = TokenService.GetToken(claims);
        return new OkObjectResult(TokenService.WriteToken(token));
    }
}