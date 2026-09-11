using CommunityToolkit.Mvvm.Input;

namespace JustMusic.Services;

public class MusicImportService
{
    private readonly string _musicFolder;

    public MusicImportService()
    {
        _musicFolder = Path.Combine(
            FileSystem.AppDataDirectory,
            "Music");

        Directory.CreateDirectory(_musicFolder);
    }


    public async Task PickAndCopyMusicAsync()
    {
        var files = await FilePicker.Default.PickMultipleAsync(
            new PickOptions
            {
                PickerTitle = "Виберіть музику"
            });

        if (files == null)
            return;

        foreach (var file in files)
        {
            if (!file.FileName.EndsWith(
                    ".mp3",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var destinationPath =
                Path.Combine(_musicFolder, file.FileName);

            await using var sourceStream =
                await file.OpenReadAsync();

            await using var destinationStream =
                File.Create(destinationPath);

            await sourceStream.CopyToAsync(destinationStream);
        }
    }

    

}

