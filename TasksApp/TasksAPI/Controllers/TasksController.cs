namespace TasksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : Controller
{
    private readonly List<TaskDto> _tasks = new List<TaskDto>()
    {
        new TaskDto { Id = 1, Name = "Task 1", Type = TaskTypeEnum.Generic },
        new TaskDto { Id = 4, Name = "Default Task 1", Type = TaskTypeEnum.Default },
        new TaskDto { Id = 5, Name = "Default Task 2", Type = TaskTypeEnum.Default },
        new TaskDto { Id = 2, Name = "Basic Task 1", Type = TaskTypeEnum.Basic },
        new TaskDto { Id = 3, Name = "Basic Task 2", Type = TaskTypeEnum.Basic }
    };

    [HttpGet("alltasks")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllTasks()
    {
        return Ok(_tasks);
    }

    [HttpGet("basictasks")]
    [Authorize]
    public async Task<IActionResult> GetBasicTasks()
    {
        return Ok(_tasks.Where(t => t.Type == TaskTypeEnum.Basic).ToList());
    }

    [HttpGet("defaulttasks")]
    public async Task<IActionResult> GetDefaultTasks()
    {
        return Ok(_tasks.Where(t => t.Type == TaskTypeEnum.Default).ToList());
    }
}
