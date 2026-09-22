using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JustMusic.Models;
using JustMusic.Services;
using System.Collections.ObjectModel;

namespace JustMusic.ViewModels;

public partial class PlayListViewModel : ObservableObject
{
    private Song? _currentSong;

    public event Action<Song>? SongPlayRequested;
    public event Action? SongPauseRequested;
    public event Action? SongResumeRequested;

    private readonly MusicLibraryService _musicLibraryService;
    public ObservableCollection<Song> Songs { get; } = [];
    public PlayListViewModel(MusicLibraryService musicLibraryService)
    {
        _musicLibraryService = musicLibraryService;
    }

    public async Task LoadSongsAsync()
    {
        await _musicLibraryService.LoadSongsAsync();
        Songs.Clear();

        foreach (var song in _musicLibraryService.Songs)
        {
            Songs.Add(song);
        }
    }

    [RelayCommand]
    private void PlaySong(Song song)
    {
        if (song == null)
            return;

        if (_currentSong == song)
        {
            if (song.IsPlaying)
            {
                song.IsPlaying = false;
                SongPauseRequested?.Invoke();
            }
            else
            {
                song.IsPlaying = true;
                SongResumeRequested?.Invoke();
            }
            return;
        }

        _currentSong?.IsPlaying = false;


        _currentSong = song;
        _currentSong.IsPlaying = true;

        SongPlayRequested?.Invoke(song);
    }
}