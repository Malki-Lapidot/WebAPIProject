using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;
using WebAPIProject.Interface;
using Microsoft.AspNetCore.Authorization;
using WebAPIProject.Services;

namespace WebAPIProject.Controllers;

[ApiController]
[Route("[controller]")]
public class JobsController : ControllerBase

{
    private IJobFinderService JobFinderService;
    public JobsController(IJobFinderService JobFinderService)
    {
        this.JobFinderService = JobFinderService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var jobs = JobFinderService.GetAll();
        if (jobs != null)
        {
            return Ok(jobs);
        }
        return StatusCode(500, "Data reading failed while retrieving users.");
    }

    [HttpGet("{id}")]
    public ActionResult<Job> Get(int id)
    {
        var specJob = JobFinderService.Get(id);
        if (specJob == null)
            return BadRequest("Oooops,Invalid Id!!!");
        return specJob;
    }


    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Post(Job newJob)
    {
        ObjectResult passObj = (ObjectResult)GeneralServise.GetUserPasswordFromToken(HttpContext);
        var password=passObj.Value as string;
        var createdBy = password;
        JobFinderService.Post(newJob, createdBy);
        return CreatedAtAction(nameof(Post), new { newJobId = newJob.JobID }, newJob);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public IActionResult Put(int id, Job newJob)
    {
        ObjectResult typeObj = (ObjectResult)GeneralServise.GetUserTypeFromToken(HttpContext);
        var type=typeObj.Value as string;
        ObjectResult passObj = (ObjectResult)GeneralServise.GetUserPasswordFromToken(HttpContext);
        var password=passObj.Value as string;
        if (type.Equals("SuperAdmin") || (type.Equals("Admin") && JobFinderService.Get(id).CreatedBy.Equals(password)))
        {
            if (id != newJob.JobID)
            {               
                return BadRequest("Not Valid Job Id!!!");
            }
            var jobToUpdate = JobFinderService.Get(id);
            if (jobToUpdate == null)
            {
                return BadRequest("Oooops,Invalid Job Id!!!");
            }
            return JobFinderService.Put(newJob);
        }
        return Forbid("You do not have permission to modify this job.");
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public IActionResult Delete(int id)
    {
        ObjectResult typeObj = (ObjectResult)GeneralServise.GetUserTypeFromToken(HttpContext);
        var type=typeObj.Value as string;
        ObjectResult passObj = (ObjectResult)GeneralServise.GetUserPasswordFromToken(HttpContext);
        var password=passObj.Value as string;
        if (type.Equals("SuperAdmin") || (type.Equals("Admin") && JobFinderService.Get(id).CreatedBy.Equals(password)))
        {
            var jobToDelete = JobFinderService.Get(id);
            if (jobToDelete == null)
                return BadRequest("Oooops, Invalid Job Id!!!");
            return JobFinderService.Delete(jobToDelete);
        }
        return Forbid("You do not have permission to delete this job.");
    }


}

