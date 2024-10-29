using task_api.Models;

namespace task_api.Repositories;

public interface ITaskRepository
{
    Tasks CreateTask(Tasks newTask);

    IEnumerable<Tasks> GetAllTasks();

    Tasks GetTaskById(int taskId);

    Tasks UpdateTask(Tasks updatedTask);

    void DeleteTaskById(int taskId);
    IEnumerable<Tasks> GetCompletedTasks();
    IEnumerable<Tasks> GetIncompleteTasks();
}