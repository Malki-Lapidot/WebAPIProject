using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;

namespace WebAPIProject.Interface
{
    public interface IJobFinderService
    {
        IEnumerable<Job> GetAll();
        Job? Get(int id);
        IActionResult Post(Job newJob,string createdBy);
        IActionResult Put(Job newJob);
        IActionResult Delete(Job jobToDelete);
    }
}
