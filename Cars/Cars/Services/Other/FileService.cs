using System.IO;

namespace Cars.Services.Other
{
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

        public static void DeleteWithoutPermissions(DirectoryInfo dirInfo)
        {
            UpdateFileAttributes(dirInfo);
            dirInfo.Delete(true);
        }

        public static string MoveAndGetUrl(string file, string id, string path, string filename)
        {
            var basePath = Directory.GetCurrentDirectory();
            var ext = Path.GetExtension(file);
            var fileLocation = Path.Combine(path, $"{filename}_{id}{ext}");
            File.Move(Path.Combine(basePath, file ?? throw new FileNotFoundException()),
                Path.Combine(basePath, fileLocation), true);
            return fileLocation;
        }
    }
}