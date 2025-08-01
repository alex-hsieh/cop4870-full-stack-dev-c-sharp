using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectDetailViewModel
    {
        public Project? Model { get; set; }

        public ProjectDetailViewModel()
        {
            Model = new Project();
        }

        public ProjectDetailViewModel(Project model)
        {
            Model = model;
        }

        public void AddOrUpdateProject()
        {
            if (Model == null)
                return;

            // Use the service proxy to handle API calls and local list updates
            ProjectServiceProxy.Current.AddOrUpdate(Model);
        }
    }

    public class ProjectDetailsPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProjectDetailViewModel> Projects { get; set; }

        public ProjectDetailViewModel? SelectedProject { get; set; }
        public int SelectedProjectId => SelectedProject?.Model?.Id ?? 0;

        public ProjectDetailsPageViewModel()
        {
            RefreshProjects();
        }

        public void RefreshProjects()
        {
            // Refresh data from server first
            ProjectServiceProxy.Current.Refresh();

            Projects = new ObservableCollection<ProjectDetailViewModel>(
                ProjectServiceProxy.Current.Projects.Select(p => new ProjectDetailViewModel(p)));
            NotifyPropertyChanged(nameof(Projects));
        }

        public void DeleteProject()
        {
            if (SelectedProject?.Model?.Id != null && SelectedProject.Model.Id > 0)
            {
                ProjectServiceProxy.Current.DeleteProject(SelectedProject.Model.Id);
                RefreshProjects(); // Refresh the list after deletion
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}