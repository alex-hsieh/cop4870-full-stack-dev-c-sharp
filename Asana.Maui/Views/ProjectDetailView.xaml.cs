using Asana.Library.Models;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(ProjectId), "ProjectId")]
public partial class ProjectDetailView : ContentPage
{
    public ProjectDetailView()
    {
        InitializeComponent();
    }

    public int ProjectId { get; set; }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ProjectsPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ProjectDetailViewModel)?.AddOrUpdateProject();
        Shell.Current.GoToAsync("//ProjectsPage");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        Project? project = null;

        if (ProjectId > 0)
        {
            // Editing existing project
            project = Asana.Library.Services.ProjectServiceProxy.Current.Projects
                .FirstOrDefault(p => p.Id == ProjectId);
        }

        if (project != null)
        {
            BindingContext = new ProjectDetailViewModel(project);
        }
        else
        {
            // Creating new project
            BindingContext = new ProjectDetailViewModel();
        }
    }
}