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

            var service = ProjectServiceProxy.Current;
            var existing = service.Projects.FirstOrDefault(p => p.Id == Model.Id);
            if (existing != null)
            {
                // Update existing project
                existing.Name = Model.Name;
                existing.Description = Model.Description;
                existing.CompletePercent = Model.CompletePercent;
                existing.ToDosListP = Model.ToDosListP;
                // Add other property updates as needed
            }
            else
            {
                // Add new project
                service.Projects.Add(Model);
            }
        }
    }

    public class ProjectDetailsPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProjectDetailViewModel> Projects { get; set; }

        public ProjectDetailViewModel? SelectedProject { get; set; }
        public int SelectedProjectId => SelectedProject?.Model?.Id ?? 0;

        public ProjectDetailsPageViewModel()
        {
            Projects = new ObservableCollection<ProjectDetailViewModel>(
                ProjectServiceProxy.Current.Projects.Select(p => new ProjectDetailViewModel(p)));
        }

        public void RefreshProjects()
        {
            Projects = new ObservableCollection<ProjectDetailViewModel>(
                ProjectServiceProxy.Current.Projects.Select(p => new ProjectDetailViewModel(p)));
            NotifyPropertyChanged(nameof(Projects));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}