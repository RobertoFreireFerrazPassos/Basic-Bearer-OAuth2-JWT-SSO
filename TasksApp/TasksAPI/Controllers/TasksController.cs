namespace TasksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : Controller
{
    [HttpGet("alltasks")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllTasks()
    {
        return Ok(new List<TaskDto>()
        {
            new TaskDto { Id = 1, Name = "Task 1"},
            new TaskDto { Id = 2, Name = "Basic Task 1"},
            new TaskDto { Id = 3, Name = "Basic Task 2"}
        });
    }

    [HttpGet("basictasks")]
    [Authorize]
    public async Task<IActionResult> GetBasicTasks()
    {
        return Ok(new List<TaskDto>()
        {
            new TaskDto { Id = 2, Name = "Basic Task 1"},
            new TaskDto { Id = 3, Name = "Basic Task 2"}
        });
    }

    [HttpGet("defaulttasks")]
    public async Task<IActionResult> GetDefaultTasks()
    {
        return Ok(new List<TaskDto>()
        {
            new TaskDto { Id = 4, Name = "Default Task 1"},
            new TaskDto { Id = 5, Name = "Default Task 2"}
        });
    }
}
