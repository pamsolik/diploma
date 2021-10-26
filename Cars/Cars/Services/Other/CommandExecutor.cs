using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cars.Services.Other
{
    public static class CommandExecutor
    {
        public static Task ExecuteCommandAsync(object command, string projectDir)
        {
            return Task.Run(() =>
            {
                try
                {
                    var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command)
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = projectDir
                    };

                    var proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStartInfo;
                    
                    proc.Start();

                    var result = proc.StandardOutput.ReadToEnd();

                    proc.WaitForExit();

                    Console.WriteLine(result);
                }
                catch (Exception objException)
                {
                    // ignored
                    // TODO: Maybe log
                }
            });
        }
    }
}