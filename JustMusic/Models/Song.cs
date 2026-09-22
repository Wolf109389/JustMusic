using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace JustMusic.Models;

public partial class Song : ObservableObject
{
    [PrimaryKey, AutoIncrement]   
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty; 

    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    //public string DisplayDuration => Duration.ToString(@"mm\:ss");

    private bool _isPlaying = false;

    [Ignore]
    public bool IsPlaying
    {
        get => _isPlaying;
        set => SetProperty(ref _isPlaying, value);
    }
}
