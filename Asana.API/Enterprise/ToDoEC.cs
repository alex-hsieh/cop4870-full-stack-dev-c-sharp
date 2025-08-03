using Api.ToDoApplication.Persistence;
using Asana.Library.Models;

namespace Asana.API.Enterprise
{
    public class ToDoEC
    {
        public ToDoEC()
        {

        }

        public IEnumerable<ToDo> GetToDos()
        {
            return Filebase.Current.ToDos.Take(100);
        }

        public ToDo? GetById(int id)
        {
            return GetToDos().FirstOrDefault(t => t.Id == id);
        }

        public ToDo? Delete(int id)
        {
            var toDoToDelete = GetById(id);

            if (toDoToDelete != null)
            {
                Filebase.Current.Delete(toDoToDelete);
            }

            return toDoToDelete;
        }

        public ToDo? AddOrUpdate(ToDo? toDo)
        {
            if (toDo == null)
                return null;

            Filebase.Current.AddOrUpdate(toDo);
            return toDo;
        }
    }
}