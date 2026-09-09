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
            try
            {
                var file = TagLib.File.Create(filePath);
                var song = new Song
                {
                    FilePath = filePath,
                    Title = file.Tag.Title ?? Path.GetFileNameWithoutExtension(filePath),
                    Artist = string.Join(", ", file.Tag.Performers) ?? "",
                    Album = file.Tag.Album ?? "",
                    Duration = file.Properties.Duration,
                };

                Songs.Add(song);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                $"Не вдалося прочитати файл: {filePath}");

                System.Diagnostics.Debug.WriteLine(
                    $"Помилка: {ex.Message}");
            }
        }

        await Task.CompletedTask;
        
    }
}
