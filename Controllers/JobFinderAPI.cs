using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers;

[ApiController]
[Route("[controller]")]

public class JobFinderAPIController : ControllerBase
{
    private static List<JobFinderAPI> JobList;

    static JobFinderAPIController(){
        JobList=new List<JobFinderAPI>{
            new JobFinderAPI{JobID=111,Location="Jerusalem",JobFieldCategory="programming",Sallery=10000,JobDescription="",PostedDate=DateTime.Now},
            new JobFinderAPI{JobID=222,Location="Modiin",JobFieldCategory="programming",Sallery=1500,JobDescription="",PostedDate=DateTime.Now}
        };
    }

    [HttpGet]
    public IEnumerable<JobFinderAPI> Get(){
        return JobList;
    }

    [HttpGet("{id}")]
    public ActionResult<JobFinderAPI> Get(int id){
        var specJob=JobList.FirstOrDefault(j=>j.JobID==id);
        if(specJob==null)
            return BadRequest("invalid id");
        return specJob;
    }

    [HttpPost]
    public ActionResult Post(JobFinderAPI newJob){
        var nextID=JobList.Max(j=>j.JobID);
        newJob.JobID=nextID+1;
        JobList.Add(newJob);
        return CreatedAtAction(nameof(Post),new{newJobID=newJob.JobID},newJob);
    }

    [HttpPut ("{id}")]
    public ActionResult Put(int id,JobFinderAPI newJob){
        var jobToUpdate=JobList.FirstOrDefault(j=>j.JobID==id);
        if(jobToUpdate==null)
            return BadRequest("invalid id");
        jobToUpdate.JobID=newJob.JobID;
        jobToUpdate.Location=newJob.Location;
        jobToUpdate.JobFieldCategory=newJob.JobFieldCategory;
        jobToUpdate.Sallery=newJob.Sallery;
        jobToUpdate.JobDescription=newJob.JobDescription;
        jobToUpdate.PostedDate=newJob.PostedDate;
        return NoContent();
    }

    [HttpDelete ("{id}")]
    public ActionResult Delete(int id){
        var jobToDelete=JobList.FirstOrDefault(j=>j.JobID==id);
        if(jobToDelete==null)
            return BadRequest("invalid id");
        JobList.Remove(jobToDelete);
        return Ok(jobToDelete);
    }

}
