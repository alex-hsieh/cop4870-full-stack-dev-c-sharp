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
    public class ToDoController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ToDoDto> Get()
        {
            return new ToDoEC().GetToDos()
                .Select(ToDoDto.FromModel);
        }

        [HttpGet("{id}")]
        public ActionResult<ToDoDto> GetById(int id)
        {
            var toDo = new ToDoEC().GetById(id);
            if (toDo == null)
                return NotFound();
            return ToDoDto.FromModel(toDo);
        }

        [HttpDelete("{id}")]
        public ActionResult<ToDoDto> Delete(int id)
        {
            var deleted = new ToDoEC().Delete(id);
            if (deleted == null)
                return NotFound();
            return ToDoDto.FromModel(deleted);
        }

        [HttpPost]
        public ActionResult<ToDoDto> AddOrUpdate([FromBody] ToDoDto? toDoDto)
        {
            if (toDoDto == null)
                return BadRequest();

            var toDo = toDoDto.ToModel();
            var result = new ToDoEC().AddOrUpdate(toDo);
            if (result == null)
                return BadRequest();
            return ToDoDto.FromModel(result);
        }
    }
}