using System;
using System.IO;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace Cars.Services.Other
{
    public static class RepositoryLoader
    {
        public static async Task Clone(string url, string workingDirectory)
        {
            Repository.Clone(url, workingDirectory);
        }
    }
}