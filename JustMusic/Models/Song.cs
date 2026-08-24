namespace JustMusic.Models;

public class Song
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string AudioPath { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty; 

    public TimeSpan Duration { get; set; }
    public string DisplayDuration => Duration.ToString(@"mm\:ss");
}
