﻿namespace Cars.Models.View;

public class FilePath
{
    public FilePath(string dbPath)
    {
        DbPath = dbPath;
    }

    public string DbPath { get; set; }
}