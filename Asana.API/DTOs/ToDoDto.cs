using System;
using Asana.Library.Models;

namespace Asana.API.DTOs
{
    public class ToDoDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Priority { get; set; }
        public bool? IsCompleted { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? DueDate { get; set; }

        // Map from Model to DTO
        public static ToDoDto FromModel(ToDo model)
        {
            if (model == null) return null!;
            return new ToDoDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Priority = model.Priority,
                IsCompleted = model.IsCompleted,
                ProjectId = model.ProjectId,
                DueDate = model.DueDate
            };
        }

        // Map from DTO to Model
        public ToDo ToModel()
        {
            return new ToDo
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Priority = this.Priority,
                IsCompleted = this.IsCompleted,
                ProjectId = this.ProjectId,
                DueDate = this.DueDate
            };
        }
    }
}