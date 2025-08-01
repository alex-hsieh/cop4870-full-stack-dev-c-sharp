using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Asana.Library.Models;
using Asana.Library.Services;

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProjectDetailViewModel> Projects { get; set; }

        public ProjectDetailViewModel? SelectedProject { get; set; }
        public int SelectedProjectId => SelectedProject?.Model?.Id ?? 0;

        public ProjectsPageViewModel()
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