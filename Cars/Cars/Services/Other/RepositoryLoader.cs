using LibGit2Sharp;

namespace Cars.Services.Other;

public static class RepositoryLoader
{
    public static void Clone(string url, string workingDirectory)
    {
        Repository.Clone(url, workingDirectory);
    }
}