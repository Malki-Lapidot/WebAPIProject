using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;

namespace WebAPIProject.Interface
{

    public interface IJobFinderService
    {
        IEnumerable<JobFinderAPI> GetAll();

        JobFinderAPI? Get(int id);

        void Post(JobFinderAPI newJob);

        void Delete(JobFinderAPI jobToDelete);
        void Put(JobFinderAPI jobToUpdate, JobFinderAPI newJob);

    }
}
