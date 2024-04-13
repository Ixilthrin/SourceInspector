using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceInspector
{
    public static class CommitInfoProvider
    {
        public static void PopulateFileChanged(CommitInfo info, ToolProperties props)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = props.CommandPath,
                    WorkingDirectory = props.WorkingDirectory,
                    Arguments = "show --name-only --pretty=format: " + info.Hash,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (!props.OnlyShowSourceFiles || line.EndsWith(".cs") || line.EndsWith(".xaml") || line.EndsWith(".py") || line.EndsWith(".resx"))
                {
                    if (!props.IgnoreAssemblyInfo || !line.EndsWith("AssemblyInfo.cs"))
                    {
                        info.FilesChanged.Add(line);
                    }
                }
            }

            proc.Close(); ;
        }
    }
}
