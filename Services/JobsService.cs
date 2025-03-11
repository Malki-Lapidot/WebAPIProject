using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPIProject.Interface;
using WebAPIProject.Models;


namespace WebAPIProject.Services
{

    public class JobsService : IJobFinderService
    {
        private readonly string jsonFilePath = "./Json/Jobs.json";
        public IEnumerable<Job> GetAll()
        {
            var objectList = GeneralServise.ReadFromJsonFile(jsonFilePath, "job");
            return objectList.Cast<Job>();
        }
        public Job? Get(int id)
        {
            var jobs = GeneralServise.ReadFromJsonFile(jsonFilePath, "job").Cast<Job>().ToList();
            var specJob = jobs.Find(x => x.JobID == id);
            return specJob;
        }

        public IActionResult Post(Job newJob, string createdBy)
        {
            newJob.CreatedBy = createdBy;
            var nextId = GeneralServise.ReadFromJsonFile(jsonFilePath, "job").Cast<Job>().ToList().Max(n => n.JobID);
            newJob.JobID = nextId + 1;
            var writeResult = GeneralServise.WriteToJsonFile(jsonFilePath, newJob, "job");
            if (writeResult is ObjectResult obj && obj.StatusCode == 404)
                return new ObjectResult("File not found") { StatusCode = 404 };
            return new OkObjectResult("Job Added Successfuly");
        }

        public IActionResult Put(Job newJob)
        {
            var deleteResult = Delete(newJob);
            if (deleteResult is ObjectResult obj && obj.StatusCode == 400)
            {
                return new ObjectResult("Job ID not found") { StatusCode = 400 };
            }
            return GeneralServise.WriteToJsonFile(jsonFilePath, newJob, "job");
        }
        public IActionResult Delete(Job jobToDelete)
        {
            var jobList = GeneralServise.ReadFromJsonFile(jsonFilePath, "job").Cast<Job>().ToList();
            bool isRemoved = jobList.Remove(jobList.FirstOrDefault(j => j.JobID == jobToDelete.JobID));

            if (!isRemoved)
            {
                return new ObjectResult("Job ID not found.") { StatusCode = 400 };
            }
            string jobJson = JsonConvert.SerializeObject(jobList, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jobJson);
            return new ObjectResult("Job deleted successfully") { StatusCode = 201 };
        }

    }

    public static class JobsServiceHelper
    {
        public static void AddJobsService(this IServiceCollection services)
        {
            services.AddSingleton<IJobFinderService, JobsService>();
        }
    }
}