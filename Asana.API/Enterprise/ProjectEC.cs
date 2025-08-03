using Api.ToDoApplication.Persistence;
using Asana.Library.Models;

namespace Asana.API.Enterprise
{
    public class ProjectEC
    {
        public IEnumerable<Project>? Get(bool Expand = false)
        {
            // You need to implement GetProjects in Filebase for this to work
            return Filebase.Current.GetProjects(Expand)?.Take(100);
        }

        public Project? GetById(int id)
        {
            // You need to implement GetProjects in Filebase for this to work
            return Filebase.Current.GetProjects(true)?.FirstOrDefault(p => p.Id == id);
        }

        public Project? AddOrUpdate(Project? project)
        {
            if (project == null)
            {
                return project;
            }

            // You need to implement AddOrUpdateProject in Filebase for this to work
            Filebase.Current.AddOrUpdateProject(project);
            return project;
        }

        public Project? Delete(int id)
        {
            var projectToDelete = GetById(id);
            if (projectToDelete != null)
            {
                // You need to implement DeleteProject in Filebase for this to work
                Filebase.Current.DeleteProject(projectToDelete);
            }
            return projectToDelete;
        }
    }
}