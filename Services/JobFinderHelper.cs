using WebAPIProject.Interface;
using WebAPIProject.services;

namespace WebAPIProject.Service
{

    public static class JobFinderHelper
    {
        public static void AddJobFinderService(this IServiceCollection service){
            service.AddSingleton<IJobFinderService, JobFinderService>();
        }
    }
}