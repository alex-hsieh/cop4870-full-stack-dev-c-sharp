using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

public partial class ProjectsView : ContentPage
{
    private ProjectsPageViewModel _viewModel;

    public ProjectsView()
    {
        InitializeComponent();
        _viewModel = new ProjectsPageViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh projects when page appears (e.g., after adding/editing a project)
        _viewModel.RefreshProjects();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//ProjectDetails");
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var selectedId = _viewModel.SelectedProjectId;
        if (selectedId > 0)
        {
            Shell.Current.GoToAsync($"//ProjectDetails?ProjectId={selectedId}");
        }
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        _viewModel.DeleteProject();
    }
}