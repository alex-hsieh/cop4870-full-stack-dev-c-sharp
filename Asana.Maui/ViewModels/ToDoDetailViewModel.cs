using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ToDoDetailViewModel : INotifyPropertyChanged
    {
        private List<Project> projects;
        public List<Project> Projects
        {
            get => projects;
            set
            {
                if (projects != value)
                {
                    projects = value;
                    OnPropertyChanged(nameof(Projects));
                }
            }
        }

        private Project? selectedProject;
        public Project? SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    Model.ProjectId = selectedProject?.Id;
                    SetProjectName(Model.ProjectId);
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        private void LoadProjects()
        {
            // Add a dummy "No Project" option
            var allProjects = ProjectServiceProxy.Current.Projects.ToList();
            allProjects.Insert(0, new Project { Id = 0, Name = "No Project" });
            Projects = allProjects;
        }

        private void SetSelectedProject()
        {
            SelectedProject = Projects.FirstOrDefault(p => p.Id == (Model.ProjectId ?? 0));
        }

        public ToDoDetailViewModel()
        {
            Model = new ToDo();
            LoadProjects();
            SetSelectedProject();
            SetProjectName(Model.ProjectId);
            HookModel();
            DeleteCommand = new Command(DoDelete);
        }

        public ToDoDetailViewModel(int toDoId, int projectId)
        {
            Model = ToDoServiceProxy.Current.GetById(toDoId) ?? new ToDo { ProjectId = projectId };
            LoadProjects();
            SetSelectedProject();
            SetProjectName(Model.ProjectId);
            HookModel();
            DeleteCommand = new Command(DoDelete);
        }

        public ToDoDetailViewModel(ToDo? model)
        {
            Model = model ?? new ToDo();
            LoadProjects();
            SetSelectedProject();
            SetProjectName(Model.ProjectId);
            HookModel();
            DeleteCommand = new Command(DoDelete);
        }

        private void SetProjectName(int? projectId)
        {
            if (!projectId.HasValue || projectId.Value == 0)
            {
                ProjectName = "No Project";
            }
            else
            {
                var project = ProjectServiceProxy.Current.GetById(projectId.Value);
                ProjectName = project?.Name ?? "Unknown Project";
            }
            OnPropertyChanged(nameof(ProjectName));
        }

        private void HookModel()
        {
            if (Model != null)
            {
                Model.PropertyChanged -= Model_PropertyChanged;
                Model.PropertyChanged += Model_PropertyChanged;
            }
        }

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ToDo.IsCompleted))
            {
                OnPropertyChanged(nameof(Model));
                OnPropertyChanged(nameof(Model.IsCompleted));
                ToDoServiceProxy.Current.AddOrUpdate(Model);
            }
            
            if (e.PropertyName == nameof(ToDo.ProjectId))
            {
                SetProjectName(Model?.ProjectId);
            }
        }

        public void DoDelete()
        {
            ToDoServiceProxy.Current.DeleteToDo(Model?.Id ?? 0);
        }

        private ToDo? model;
        public ToDo? Model
        {
            get => model;
            set
            {
                if (model != value)
                {
                    if (model != null)
                        model.PropertyChanged -= Model_PropertyChanged;
                    model = value;
                    HookModel();
                    SetProjectName(model?.ProjectId);
                    OnPropertyChanged(nameof(Model));
                }
            }
        }

        public ICommand? DeleteCommand { get; set; }

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
                    OnPropertyChanged(nameof(SelectedPriority));
                }
            }
        }

        public DateTime DueDate
        {
            get => Model?.DueDate ?? DateTime.Today;
            set
            {
                if (Model != null && Model.DueDate != value)
                {
                    Model.DueDate = value;
                    OnPropertyChanged(nameof(DueDate));
                }
            }
        }

        public void AddOrUpdateToDo()
        {
            ToDoServiceProxy.Current.AddOrUpdate(Model);
        }

        public string PriorityDisplay
        {
            set
            {
                if (Model == null)
                {
                    return;
                }

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

        private string projectName = string.Empty;
        public string ProjectName
        {
            get => projectName;
            set
            {
                if (projectName != value)
                {
                    projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}