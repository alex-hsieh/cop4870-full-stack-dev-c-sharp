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
    public class ProjectViewModel : INotifyPropertyChanged
    {
        public Project? Model { get; set; }

        public ObservableCollection<ToDoDetailViewModel> ToDos
        {
            get
            {
                if (Model == null || Model.ToDosListP == null)
                {
                    return new ObservableCollection<ToDoDetailViewModel>();
                }

                return new ObservableCollection<ToDoDetailViewModel>(
                    Model.ToDosListP.Select(t => new ToDoDetailViewModel(t)));
            }
        }
        public ICommand? ToggleToDoVisibility { get; set; }

        private Visibility toDoVisibility;
        public Visibility ToDoVisibility {
            get
            {
                return toDoVisibility;
            }
            set
            {
                if (toDoVisibility != value)
                {
                    toDoVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void DoToggleToDoVisibility()
        {
            if (ToDoVisibility == Visibility.Collapsed)
            {
                ToDoVisibility = Visibility.Visible;
            } else
            {
                ToDoVisibility = Visibility.Collapsed;
            }
            NotifyPropertyChanged(nameof(ToDoVisibility));
        }

        public ProjectViewModel()
        {
            Model = new Project();
            ToDoVisibility = Visibility.Visible;
            ToggleToDoVisibility = new Command(DoToggleToDoVisibility);
        }
        public ProjectViewModel(Project? model)
        {
            Model = model;
            ToDoVisibility = Visibility.Visible;
            ToggleToDoVisibility = new Command(DoToggleToDoVisibility);
        }

        public override string ToString()
        {
            return $"{Model?.Id ?? -1}. {Model?.Name}";
        }

        public string? Description
        {
            get => Model?.Description;
        }

        public int ToDoCount
        {
            get => Model?.ToDosListP?.Count ?? 0;
        }

        public double CompletionPercentage
        {
            get
            {
                if (Model?.ToDosListP == null || Model.ToDosListP.Count == 0)
                    return 0;
                int completed = Model.ToDosListP.Count(t => t.IsCompleted == true);
                return (double)completed / Model.ToDosListP.Count * 100;
            }
        }
        public IEnumerable<string> ToDoNames => Model?.ToDosListP?.Select(t => t.Name ?? string.Empty) ?? Enumerable.Empty<string>();

        public ObservableCollection<ProjectViewModel> Projects { get; set; }

        private ProjectViewModel? selectedProject;
        public ProjectViewModel? SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(SelectedProjectId));
                }
            }
        }

        public int SelectedProjectId => SelectedProject?.Model?.Id ?? 0;
    }
}
