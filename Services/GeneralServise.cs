using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPIProject.Models;

namespace WebAPIProject.Services;

public static class GeneralServise
{
    public static List<Object> ReadFromJsonFile(string jsonFilePath, string kind)
    {
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"The file {jsonFilePath} was not found.");
        }
        string jsonContetnt = File.ReadAllText(jsonFilePath);
        if (kind.Equals("user"))
        {
            var list = JsonConvert.DeserializeObject<List<User>>(jsonContetnt);
            return list.Cast<Object>().ToList();
        }
        else if (kind.Equals("job"))
        {
            var list = JsonConvert.DeserializeObject<List<Job>>(jsonContetnt);
            return list.Cast<Object>().ToList();
        }
        else
        {
            throw new ArgumentException("Invalid kind specified. Expected 'user' or 'job'.");
        }
    }
    public static ActionResult WriteToJsonFile(string jsonFilePath,Object newObject,string kind)
    {
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"The file {jsonFilePath} was not found.");
        }
        var list = ReadFromJsonFile(jsonFilePath,kind);
        list.Add(newObject);
        string json = JsonConvert.SerializeObject(list, Formatting.Indented);
        File.WriteAllText(jsonFilePath, json);
        return new ObjectResult("DB update successfuly"){StatusCode=201};
    }


    public static string? GetCurrentToken(HttpContext httpContext)
    {
        var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring(7);
            return token;
        }
        return null;
    }

    public static ActionResult GetUserPasswordFromToken(HttpContext httpContext)
    {
        var currentToken = GetCurrentToken(httpContext);
        if (currentToken == null)
        {
            return new ObjectResult("Unauthorized: Invalid or missing token"){StatusCode=401};
        }
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(currentToken) as JwtSecurityToken;
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "password")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return new ObjectResult("User ID not found in token"){StatusCode=400};
        return new ObjectResult(userIdClaim){StatusCode=200};
    }


    public static ActionResult GetUserTypeFromToken(HttpContext httpContext)
    {
        var currentToken = GetCurrentToken(httpContext);
        if (currentToken == null)
        {
            return new ObjectResult("Unauthorized: Invalid or missing token"){StatusCode=401};
        }
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(currentToken) as JwtSecurityToken;
        var userRoleClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "type")?.Value;
        if (string.IsNullOrEmpty(userRoleClaim))
            return new ObjectResult("User ID not found in token"){StatusCode=400};
        return new ObjectResult(userRoleClaim){StatusCode=200};
    }
}