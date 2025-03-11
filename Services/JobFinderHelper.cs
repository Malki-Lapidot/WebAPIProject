using WebAPIProject.Interface;
using WebAPIProject.services;

namespace WebAPIProject.Services
{

    public static class JobFinderHelper
    {
        public static void AddJobFinderService(this IServiceCollection service){
            service.AddSingleton<IJobFinderService, JobsService>();
            service.AddSingleton<IUserFinderServise, UsersServise>();
            service.AddSingleton<ITokenService, TokenService>();
        }
    }
}