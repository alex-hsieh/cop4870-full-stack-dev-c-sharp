using Asana.Library.Models;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(ProjectId), "toDoId")]
public partial class ProjectDetailView : ContentPage
{
    public ProjectDetailView()
    {
        InitializeComponent();

    }
    public int ProjectId { get; set; }
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ToDoDetailViewModel)?.AddOrUpdateToDo();
        Shell.Current.GoToAsync("//ProjectsPage");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ToDoDetailViewModel(ProjectId);
    }
}