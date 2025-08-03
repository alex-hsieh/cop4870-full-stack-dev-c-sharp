using Asana.API.DTOs;
using Asana.API.Enterprise;
using Asana.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Asana.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ProjectDto> Get()
        {
            return new ProjectEC().Get()
                ?.Select(ProjectDto.FromModel) ?? Enumerable.Empty<ProjectDto>();
        }

        [HttpGet("Expand")]
        public IEnumerable<ProjectDto> GetExpand()
        {
            return new ProjectEC().Get(true)
                ?.Select(ProjectDto.FromModel) ?? Enumerable.Empty<ProjectDto>();
        }

        [HttpGet("{id}")]
        public ActionResult<ProjectDto> GetById(int id)
        {
            var project = new ProjectEC().GetById(id);
            if (project == null)
                return NotFound();
            return ProjectDto.FromModel(project);
        }

        [HttpDelete("{id}")]
        public ActionResult<ProjectDto> Delete(int id)
        {
            var deleted = new ProjectEC().Delete(id);
            if (deleted == null)
                return NotFound();
            return ProjectDto.FromModel(deleted);
        }

        [HttpPost]
        public ActionResult<ProjectDto> AddOrUpdate([FromBody] ProjectDto? projectDto)
        {
            if (projectDto == null)
                return BadRequest();

            var project = projectDto.ToModel();
            var result = new ProjectEC().AddOrUpdate(project);
            if (result == null)
                return BadRequest();
            return ProjectDto.FromModel(result);
        }
    }
}