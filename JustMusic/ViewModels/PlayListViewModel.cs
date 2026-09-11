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

    public ObservableCollection<Song> Songs { get; } = [];
    private readonly MusicLibraryService _musicLibraryService;
    private readonly MusicImportService _musicImportService;

    public PlayListViewModel(MusicLibraryService musicLibraryService, MusicImportService musicImportService )
    {
        _musicLibraryService = musicLibraryService;
        _musicImportService = musicImportService;
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
            if (song.isPlaying)
            {
                song.isPlaying = false;
                SongPauseRequested?.Invoke();
            }
            else
            {
                song.isPlaying = true;
                SongResumeRequested?.Invoke();
            }
            return;
        }

        if (_currentSong != null)
            _currentSong.isPlaying = false; 

        _currentSong = song;
        _currentSong.isPlaying = true;

        SongPlayRequested?.Invoke(song);
    }

    [ObservableProperty]
    private bool isAddMusicMenuVisible = false;

    [RelayCommand]
    private void ToggleAddMusicMenu()
    {
        IsAddMusicMenuVisible = !IsAddMusicMenuVisible;
    }

    [RelayCommand]
    private async Task CopyMusicFilesAsync()
    {
        await _musicImportService.PickAndCopyMusicAsync();

        await LoadSongsAsync();

        IsAddMusicMenuVisible = false;
    }
}