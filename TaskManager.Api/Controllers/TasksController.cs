using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Models;
using TaskManager.Api.Models.Data;
using TaskManager.Api.Models.Services;
using TaskManager.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly TasksService _tasksService;

        public TasksController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _tasksService = new TasksService(db);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommonModel>>> GetTasksByDesk(int deskId)
        {
            var result = await _tasksService.GetAll(deskId).ToListAsync();
            return result != null ? Ok(result) : NoContent();
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<CommonModel>>> GetTasksForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var result = await _tasksService.GetTaskForUser(user.Id).ToListAsync();
                return result != null ? Ok(result) : NoContent();
            }
            return Unauthorized(Array.Empty<CommonModel>());


        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var task = _tasksService.Get(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskModel model)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (model != null)
                {
                    model.CreatorId = user.Id;
                    bool result = _tasksService.Create(model);
                    return result ? Ok(result) : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] TaskModel taskModel)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (taskModel != null)
                {
                    bool result = _tasksService.Update(id, taskModel);
                    return result ? Ok(result) : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _tasksService.Delete(id);
            return result ? Ok(result) : NotFound();
        }
    }
}
