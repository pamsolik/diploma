﻿namespace Services.Other;

public static class FileService
{
    private static void UpdateFileAttributes(DirectoryInfo dInfo)
    {
        dInfo.Attributes &= ~FileAttributes.ReadOnly;

        foreach (var file in dInfo.GetFiles()) file.Attributes &= ~FileAttributes.ReadOnly;

        foreach (var subDir in dInfo.GetDirectories()) UpdateFileAttributes(subDir);
    }

    public static void EnsureDirectoryIsCreated(DirectoryInfo dirInfo)
    {
        if (dirInfo.Exists) DeleteWithoutPermissions(dirInfo);

        dirInfo.Create();
    }

    public static void Create(string? dir)
    {
        if (!File.Exists(dir) && dir is not null) Directory.CreateDirectory(dir);
    }

    public static void CreateNeededFileStructure(string dir)
    {
        Create(Path.Combine(dir, @"Resources"));
        Create(Path.Combine(dir, @"Resources/Temp"));
        Create(Path.Combine(dir, @"Resources/Files"));
        Create(Path.Combine(dir, @"Resources/Files/CL"));
        Create(Path.Combine(dir, @"Resources/Files/CV"));
        Create(Path.Combine(dir, @"Resources/Images/Defaults"));
        Create(Path.Combine(dir, @"Resources/Images/Profile"));
        Create(Path.Combine(dir, @"Resources/Images/Thumbnails"));
    }


    public static void DeleteWithoutPermissions(DirectoryInfo dirInfo)
    {
        UpdateFileAttributes(dirInfo);
        dirInfo.Delete(true);
    }

    public static string? MoveAndGetUrl(string? file, string id, string path, string filename)
    {
        var basePath = Directory.GetCurrentDirectory();
        var ext = Path.GetExtension(file);
        var fileLocation = Path.Combine(path, $"{filename}_{id}{ext}");
        File.Move(Path.Combine(basePath, file ?? throw new FileNotFoundException()),
            Path.Combine(basePath, fileLocation), true);
        return fileLocation;
    }

    public static (string dir, int projects) FindAllFiles(string sDir, string searchPattern, int retry = 0)
    {
        retry -= 3;
        if (retry < 0) retry = 0;
        var dirs = Directory.GetFiles(sDir, searchPattern, SearchOption.AllDirectories);
        var cnt = dirs.Length;
        var dir = cnt > 0 ? Path.GetDirectoryName(dirs.ElementAt(retry)) : sDir;
        return (dir ?? sDir, cnt);
    }
}