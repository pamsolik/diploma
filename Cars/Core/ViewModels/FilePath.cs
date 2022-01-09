namespace Core.ViewModels;

public class FilePath
{
    public FilePath(string? dbPath)
    {
        DbPath = dbPath;
    }

    public string? DbPath { get; set; }
}