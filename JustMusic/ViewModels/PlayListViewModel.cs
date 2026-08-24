using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JustMusic.Models;
using JustMusic.Services;
using JustMusic.Views;
using System.Collections.ObjectModel;

namespace JustMusic.ViewModels;

public partial class PlayListViewModel : ObservableObject
{
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
}

