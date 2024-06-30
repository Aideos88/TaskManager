using TaskManager.Common.Models;

namespace TaskManager.Api.Models.Abstractions
{
    public class CommonObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[]? Photo { get; set; }

        public CommonObject()
        {
            CreatedDate = DateTime.Now;
        }

        public CommonObject(CommonModel model)
        {
            Name = model.Name;
            Description = model.Description;
            Photo = model.Photo;
            CreatedDate = DateTime.Now;
        }

    }
}
