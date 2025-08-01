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
            AvailableProjects = ProjectServiceProxy.Current.Projects.ToList();
        }

        public void DoDelete()
        {
            ToDoServiceProxy.Current.DeleteToDo(Model?.Id ?? 0);
        }

        public ToDo? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }

        // Add project support
        public List<Project> AvailableProjects { get; private set; } = new List<Project>();

        public Project? SelectedProject
        {
            get
            {
                if (Model?.ProjectId == null) return null;
                return AvailableProjects.FirstOrDefault(p => p.Id == Model.ProjectId);
            }
            set
            {
                if (Model != null)
                {
                    Model.ProjectId = value?.Id; // Aquí se asigna el ProjectId al ToDo
                }
            }
        }

        // Método simple para obtener el nombre del proyecto
        public string ProjectName
        {
            get
            {
                if (Model?.ProjectId == null) return "No Project";
                var project = AvailableProjects.FirstOrDefault(p => p.Id == Model.ProjectId);
                return project?.Name ?? "Unknown / Unassigned Project";
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
        }
    }
}