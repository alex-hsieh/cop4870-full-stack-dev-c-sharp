using System.Collections.Generic;
using System.Linq;
using Asana.Library.Models;

namespace Asana.API.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double CompletePercent { get; set; }
        public List<ToDoDto>? ToDos { get; set; }

        // Map from Model to DTO
        public static ProjectDto FromModel(Project model)
        {
            if (model == null) return null!;
            return new ProjectDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CompletePercent = model.CompletePercent,
                ToDos = model.ToDosListP?.Select(ToDoDto.FromModel).ToList()
            };
        }

        // Map from DTO to Model
        public Project ToModel()
        {
            return new Project
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                CompletePercent = this.CompletePercent,
                ToDosListP = this.ToDos?.Select(dto => dto.ToModel()).ToList()
            };
        }
    }
}