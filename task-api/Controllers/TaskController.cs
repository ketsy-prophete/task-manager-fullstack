using task_api.Models;
using task_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace task_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskRepository _taskRepository;


        public TaskController(ILogger<TaskController> logger, ITaskRepository repository)
        {
            _logger = logger;
            _taskRepository = repository;
        }

        [HttpPost]
        public ActionResult<Task> CreateTask([FromBody] Tasks newTask)
        {
            if (!ModelState.IsValid || newTask == null)
            {
                return BadRequest();
            }

            var createdTask = _taskRepository.CreateTask(newTask);
            return CreatedAtAction(nameof(GetAllTasks), new { taskId = createdTask.TaskId }, createdTask);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Tasks>> GetAllTasks()
        {
            return Ok(_taskRepository.GetAllTasks());
        }

        [HttpGet("{taskId:int}")]
        public ActionResult<Tasks> GetTaskById(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut]
        [Route("{taskId:int}")]
        public ActionResult<Tasks> UpdateTask([FromBody] Tasks updatedTask, int taskId)
        {
            if (!ModelState.IsValid || updatedTask == null || updatedTask.TaskId != taskId)
            {
                return BadRequest();
            }

            var taskUpdate = _taskRepository.UpdateTask(updatedTask);
            if (taskUpdate == null)
            {
                return NotFound();
            }
            return Ok(taskUpdate);
        }

        [HttpDelete]
        [Route("{taskId:int}")]
        public ActionResult DeleteTaskById(int taskId)
        {
            var existingTask = _taskRepository.GetTaskById(taskId);
            if (existingTask == null)
            {
                return NotFound();
            }

            _taskRepository.DeleteTaskById(taskId);
            return NoContent();
        }

        [HttpGet("completed")]
        public IActionResult GetCompletedTasks()
        {
            var completedTasks = _taskRepository.GetCompletedTasks();
            return Ok(completedTasks);
        }

        [HttpGet("incomplete")]
        public IActionResult GetIncompleteTasks()
        {
            var incompleteTasks = _taskRepository.GetIncompleteTasks();
            return Ok(incompleteTasks);
        }

    }
}