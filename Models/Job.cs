namespace WebAPIProject.Models;

public class Job
{
    public int JobID { get; set; }

    public string? Location { get; set; }

    public string? JobFieldCategory { get; set; }

    public int Sallery { get; set; }

    public string? JobDescription { get; set; }

    public DateTime PostedDate { get; set; }

    public string? CreatedBy{ get; set; }
}
