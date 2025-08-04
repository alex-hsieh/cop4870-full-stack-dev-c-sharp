using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Asana.Library.Models;
using Asana.Library.Services;

namespace Asana.Maui.ViewModels
{
    public class ProjectDetailViewModel : INotifyPropertyChanged
    {
        public Project? Model { get; set; }

        public ObservableCollection<ToDo> ToDos { get; set; }

        public ICommand AddToDoCommand { get; }
        public List<string> IncompleteToDoNames =>
            ToDos?.Where(t => t.IsCompleted != true)
              .Select(t => t.Name ?? "Unnamed Task")
             .ToList() ?? new List<string>();

        public ProjectDetailViewModel()
        {
            Model = new Project { ToDosListP = new List<ToDo>() };
            ToDos = new ObservableCollection<ToDo>();
            AddToDoCommand = new Command(OnAddToDo);

            // Subscribe to ToDosChanged event
            ToDoServiceProxy.Current.ToDosChanged += OnToDosChanged;
        }

        public ProjectDetailViewModel(Project model)
        {
            Model = model;
            if (Model.ToDosListP == null)
                Model.ToDosListP = new List<ToDo>();
            ToDos = new ObservableCollection<ToDo>(GetProjectToDos());
            AddToDoCommand = new Command(OnAddToDo);

            // Subscribe to ToDosChanged event
            ToDoServiceProxy.Current.ToDosChanged += OnToDosChanged;
        }

        private void OnToDosChanged()
        {
            RefreshToDos();
        }

        private async void OnAddToDo()
        {
            if (Model?.Id > 0)
                await Shell.Current.GoToAsync($"///ToDoDetails?toDoId=0&projectId={Model.Id}");
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
            OnPropertyChanged(nameof(CompletionPercentage));
            OnPropertyChanged(nameof(IncompleteToDoNames)); 

        }

        public void RefreshToDos()
        {
            if (Model == null)
                return;

            var latestToDos = GetProjectToDos();

            ToDos.Clear();
            foreach (var todo in latestToDos)
                ToDos.Add(todo);

            Model.ToDosListP = latestToDos.ToList();

            OnPropertyChanged(nameof(ToDos));
            OnPropertyChanged(nameof(CompletionPercentage));
            OnPropertyChanged(nameof(ToDoNames));
            OnPropertyChanged(nameof(IncompleteToDoNames)); 
        }

        private IEnumerable<ToDo> GetProjectToDos()
        {
            if (Model == null)
                return Enumerable.Empty<ToDo>();

            // Pull from the global ToDoServiceProxy to ensure up-to-date data
            return ToDoServiceProxy.Current.ToDos
                .Where(t => t.ProjectId == Model.Id)
                .ToList();
        }

        public void AddOrUpdateProject()
        {
            if (Model == null)
                return;

            ProjectServiceProxy.Current.AddOrUpdate(Model);
        }

        // Properties for UI binding
        public double CompletionPercentage
        {
            get
            {
                if (ToDos == null || ToDos.Count == 0)
                    return 0.0;
                var completed = ToDos.Count(t => t.IsCompleted == true);
                return (double)completed / ToDos.Count * 100.0;
            }
        }

        public string Description => Model?.Description ?? string.Empty;

        public List<string> ToDoNames => ToDos?.Select(t => t.Name ?? "Unnamed Task").ToList() ?? new List<string>();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}