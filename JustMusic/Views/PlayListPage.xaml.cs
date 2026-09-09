using CommunityToolkit.Maui.Views;
using JustMusic.Models;
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

        vm.SongPlayRequested += OnSongPlayRequested;
        vm.SongPauseRequested += OnSongPauseRequested;
        vm.SongResumeRequested += OnSongResumeRequested;
    }

    private void OnSongPlayRequested(Song song)
    {
        AudioPlayer.Source = MediaSource.FromFile(song.FilePath);
        AudioPlayer.Play();
    }

    private void OnSongPauseRequested()
    {
        AudioPlayer.Pause();
    }

    private void OnSongResumeRequested()
    {
        AudioPlayer.Play();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadSongsAsync();
    }
}
