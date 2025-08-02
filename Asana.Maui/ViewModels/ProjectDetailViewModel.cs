using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectDetailViewModel : INotifyPropertyChanged
    {
        public Project? Model { get; set; }

        public ObservableCollection<ToDo> ToDos { get; set; }

        public ICommand AddToDoCommand { get; }

        public ProjectDetailViewModel()
        {
            Model = new Project { ToDosListP = new List<ToDo>() };
            ToDos = new ObservableCollection<ToDo>();
            AddToDoCommand = new Command(OnAddToDo);
        }

        public ProjectDetailViewModel(Project model)
        {
            Model = model;
            if (Model.ToDosListP == null)
                Model.ToDosListP = new List<ToDo>();
            ToDos = new ObservableCollection<ToDo>(Model.ToDosListP);
            AddToDoCommand = new Command(OnAddToDo);
        }

        private async void OnAddToDo()
        {
            if (Model?.Id > 0)
                await Shell.Current.GoToAsync($"///ToDoDetails?toDoId={Model.Id}&projectId={Model.Id}");
            else
                await Shell.Current.DisplayAlert("Error", "Please save the project before adding ToDos.", "OK");
        }

        public void AddToDo(ToDo newToDo)
        {
            if (Model?.ToDosListP == null)
                Model.ToDosListP = new List<ToDo>();
            ToDos.Add(newToDo);
            Model.ToDosListP.Add(newToDo);
            OnPropertyChanged(nameof(ToDos));
        }

        public void AddOrUpdateProject()
        {
            if (Model == null)
                return;

            ProjectServiceProxy.Current.AddOrUpdate(Model);
        }

        // Properties for UI binding
        public double CompletionPercentage => Model?.CompletePercent ?? 0.0;

        public string Description => Model?.Description ?? string.Empty;

        public List<string> ToDoNames => Model?.ToDosListP?.Select(t => t.Name ?? "Unnamed Task").ToList() ?? new List<string>();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                RefreshProjects();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}