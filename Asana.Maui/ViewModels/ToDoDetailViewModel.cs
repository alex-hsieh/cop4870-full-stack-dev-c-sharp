using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Asana.Library.Models;
using Asana.Library.Services;


namespace Asana.Maui.ViewModels
{

    public class ToDoDetailViewModel
    {
        public ToDoDetailViewModel()
        {
            Model = new ToDo();
            DeleteCommand = new Command(DoDelete);
            LoadProjects();
        }

        public ToDoDetailViewModel(int id)
        {
            Model = ToDoServiceProxy.Current.GetById(id) ?? new ToDo();
            DeleteCommand = new Command(DoDelete);
            LoadProjects();
        }

        public ToDoDetailViewModel(ToDo? model)
        {
            Model = model ?? new ToDo();
            DeleteCommand = new Command(DoDelete);
            LoadProjects();
        }

        private void LoadProjects()
        {
            AvailableProjects = new List<Project>();

            
            AvailableProjects.Add(new Project { Id = 0, Name = "No Project" });

            
            AvailableProjects.AddRange(ProjectServiceProxy.Current.Projects);
        }

        public void DoDelete()
        {
            ToDoServiceProxy.Current.DeleteToDo(Model?.Id ?? 0);
        }

        public ToDo? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }

        
        public List<Project> AvailableProjects { get; private set; } = new List<Project>();

        public Project? SelectedProject
        {
            get
            {
                if (Model?.ProjectId == null || Model.ProjectId == 0)
                {
                    
                    return AvailableProjects.FirstOrDefault(p => p.Id == 0);
                }
                return AvailableProjects.FirstOrDefault(p => p.Id == Model.ProjectId);
            }
            set
            {
                if (Model != null)
                {
                    
                    Model.ProjectId = (value?.Id == 0) ? null : value?.Id;
                }
            }
        }

        // Add this new constructor to support both ToDoId and ProjectId
        public ToDoDetailViewModel(int toDoId, int projectId)
        {
            Model = ToDoServiceProxy.Current.GetById(toDoId) ?? new ToDo();
            Model.ProjectId = projectId == 0 ? null : projectId;
            DeleteCommand = new Command(DoDelete);
            LoadProjects();
        }

        public string ProjectName
        {
            get
            {
                if (Model?.ProjectId == null || Model.ProjectId == 0)
                    return "No Project";

                var project = ProjectServiceProxy.Current.Projects.FirstOrDefault(p => p.Id == Model.ProjectId);
                return project?.Name ?? "Unknown Project";
            }
        }

        public List<int> Priorities
        {
            get
            {
                return new List<int> { 0, 1, 2, 3, 4 };
            }
        }

        public int SelectedPriority
        {
            get
            {
                return Model?.Priority ?? 4;
            }
            set
            {
                if (Model != null && Model.Priority != value)
                {
                    Model.Priority = value;
                }
            }
        }

        public string PriorityDisplay
        {
            set
            {
                if (Model == null) return;

                if (!int.TryParse(value, out int p))
                {
                    Model.Priority = -9999;
                }
                else
                {
                    Model.Priority = p;
                }
            }
            get
            {
                return Model?.Priority?.ToString() ?? string.Empty;
            }
        }

        public DateTime? DueDate
        {
            get => Model?.DueDate;
            set
            {
                if (Model != null && Model.DueDate != value)
                {
                    Model.DueDate = value;
                }
            }
        }

        public void AddOrUpdateToDo()
        {
            ToDoServiceProxy.Current.AddOrUpdate(Model);

            if (Model?.ProjectId != null && Model.ProjectId != 0)
            {
                var project = ProjectServiceProxy.Current.Projects.FirstOrDefault(p => p.Id == Model.ProjectId);
                if (project != null && project.ToDosListP != null)
                {
                    // Update the ToDo in the project's list
                    var todoInProject = project.ToDosListP.FirstOrDefault(t => t.Id == Model.Id);
                    if (todoInProject != null)
                    {
                        todoInProject.IsCompleted = Model.IsCompleted;
                    }

                    // Recalculate completion percent
                    int total = project.ToDosListP.Count;
                    int completed = project.ToDosListP.Count(t => t.IsCompleted == true);
                    project.CompletePercent = total > 0 ? (double)completed / total * 100 : 0;
                }
            }
        }
    }
}