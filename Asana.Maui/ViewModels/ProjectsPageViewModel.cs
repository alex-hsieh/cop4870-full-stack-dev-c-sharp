using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Asana.Maui.ViewModels;

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProjectDetailViewModel> Projects { get; set; }

        private ProjectDetailViewModel? selectedProject;
        public ProjectDetailViewModel? SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public ProjectsPageViewModel()
        {
            Projects = new ObservableCollection<ProjectDetailViewModel>(
                ProjectServiceProxy.Current.Projects.Select(p => new ProjectDetailViewModel(p))
            );

            // Subscribe to ToDosChanged event
            ToDoServiceProxy.Current.ToDosChanged += OnToDosChanged;
        }

        public ProjectsPageViewModel(Project project)
        {
            Projects = new ObservableCollection<ProjectDetailViewModel>
            {
                new ProjectDetailViewModel(project)
            };

            ToDoServiceProxy.Current.ToDosChanged += OnToDosChanged;
        }

        private void OnToDosChanged()
        {
            // Ensure the latest project data is loaded before refreshing
            ProjectServiceProxy.Current.Refresh();

            // Rebuild the Projects collection to reflect latest ToDo/project state
            Projects.Clear();
            foreach (var p in ProjectServiceProxy.Current.Projects)
                Projects.Add(new ProjectDetailViewModel(p));
            OnPropertyChanged(nameof(Projects));
        }

        public void RefreshProjects()
        {
            Projects.Clear();
            foreach (var p in ProjectServiceProxy.Current.Projects)
                Projects.Add(new ProjectDetailViewModel(p));
            OnPropertyChanged(nameof(Projects));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}