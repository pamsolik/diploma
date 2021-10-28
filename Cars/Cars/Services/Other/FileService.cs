using System.IO;

namespace Cars.Services.Other
{
    public static class FileService
    {
        private static void UpdateFileAttributes(DirectoryInfo dInfo)
        {
            dInfo.Attributes &= ~FileAttributes.ReadOnly;

            foreach (var file in dInfo.GetFiles())
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
            }

            foreach (var subDir in dInfo.GetDirectories())
            {
                UpdateFileAttributes(subDir);
            }
        }
        
        public static void EnsureDirectoryIsCreated(DirectoryInfo dirInfo)
        {
            if (dirInfo.Exists)
            {
                DeleteWithoutPermissions(dirInfo);
            }

            dirInfo.Create();
        }

        public static void DeleteWithoutPermissions(DirectoryInfo dirInfo)
        {
            UpdateFileAttributes(dirInfo);
            dirInfo.Delete(true);
        }
    }
}