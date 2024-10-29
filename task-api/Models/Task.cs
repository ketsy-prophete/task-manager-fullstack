namespace task_api.Models;


//contray to normal singular naming convention, I had to use plural due to system threading error
public class Tasks
{
    public int TaskId { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
}
