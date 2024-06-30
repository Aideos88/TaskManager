using Newtonsoft.Json;
using TaskManager.Api.Models.Abstractions;
using TaskManager.Common.Models;

namespace TaskManager.Api.Models
{
    public class Task : CommonObject
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[]? File { get; set; }
        public int DeskId { get; set; }
        public Desk Desk { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public User? Creator { get; set; }
        public int? ExecutorId { get; set; }
        public Task() { }
        public Task(TaskModel taskModel) : base(taskModel)
        {
            Id = taskModel.Id;
            StartDate = taskModel.CreatedDate;
            EndDate = taskModel.EndDate;
            File = taskModel.File;
            DeskId = taskModel.DeskId;
            Column = taskModel.Column;
            CreatorId = taskModel.CreatorId;
            ExecutorId = taskModel.ExecutorId;
        }

        public TaskModel ToDto()
        {
            return new TaskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreatedDate = this.CreatedDate,
                Photo = this.Photo,
                StartDate = this.CreatedDate,
                EndDate = this.EndDate,
                File = this.File,
                DeskId = this.DeskId,
                Column = this.Column,
                CreatorId = this.CreatorId,
                ExecutorId = this.ExecutorId,
            };
        }

        public TaskModel ToShortDto()
        {
            return new TaskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CreatedDate = this.CreatedDate,
                StartDate = this.CreatedDate,
                EndDate = this.EndDate,
                DeskId = this.DeskId,
                Column = this.Column,
                CreatorId = this.CreatorId,
                ExecutorId = this.ExecutorId,
            };
        }
    }
}
