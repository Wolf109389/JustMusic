using CommunityToolkit.Maui.Views;
using JustMusic.ViewModels;

namespace JustMusic.Views;

public partial class PlayListPage : ContentPage
{
    private readonly PlayListViewModel _viewModel;

    public PlayListPage(PlayListViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadSongsAsync();
    }
}
