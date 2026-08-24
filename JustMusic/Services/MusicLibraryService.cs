using JustMusic.Models;
using System.Diagnostics;

namespace JustMusic.Services;
public class MusicLibraryService
{
    private readonly string _musicFolder;

    public List<Song> Songs { get; private set; } = [];

    public MusicLibraryService()
    {
        _musicFolder = Path.Combine(
            FileSystem.AppDataDirectory,
            "Music"
        );
        
        Directory.CreateDirectory(_musicFolder);
        Debug.WriteLine($"   Music folder: {_musicFolder}");
    }
    
    public string MusicFolder => _musicFolder;

    public async Task LoadSongsAsync()
    {
        Songs.Clear();

        var files = Directory.GetFiles(
            _musicFolder, 
            "*.mp3", 
            SearchOption.AllDirectories);

            foreach (var filePath in files) 
            {
                var song = new Song
                {
                    FilePath = filePath,
                    Title = Path.GetFileNameWithoutExtension(filePath)
                };
                
                Songs.Add(song);
            }

        await Task.CompletedTask;
        
    }
}
