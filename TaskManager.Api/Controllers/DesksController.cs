﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Models.Data;
using TaskManager.Api.Models.Services;
using TaskManager.Common.Models;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly DesksService _desksService;

        public DesksController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _desksService = new DesksService(db);
        }

        [HttpGet]
        public async Task<IEnumerable<CommonModel>> GetDesksForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                return await _desksService.GetAll(user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var desk = _desksService.Get(id);
            return desk == null ? NotFound() : Ok(desk);
        }

        [HttpGet("project")]
        public async Task<IEnumerable<CommonModel>> GetProjectDesks(int projectId)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                return await _desksService.GetProjectDesks(projectId, user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }

        [HttpPost]
        public IActionResult Create([FromBody] DeskModel deskModel)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    deskModel.AdminId = user.Id;
                    bool result = _desksService.Create(deskModel);
                    return result ? Ok(result) : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskModel deskModel)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    bool result = _desksService.Update(id, deskModel);
                    return result ? Ok(result) : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _desksService.Delete(id);
            return result ? Ok(result) : NotFound();
        }
    }
}
