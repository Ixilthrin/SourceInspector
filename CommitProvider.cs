using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceInspector
{
    public static class CommitProvider
    {
        public static List<CommitInfo> GetCommits(ToolProperties props)
        {
            List<CommitInfo> commits = new List<CommitInfo>();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = props.CommandPath,
                    WorkingDirectory = props.WorkingDirectory,
                    Arguments = "log --all --pretty=format:\"%h`%an`%cd`%s\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                var values = line.Split('`');

                if (values.Length > 4)
                    throw new Exception("Special character | exists inside meta data");

                bool addCommit = true;

                if (values[1] == "ana_build" || values[1] == "Jenkins")
                {
                    addCommit = false;
                }

                if (props.UserNameSearchString != "*" && !values[1].Contains(props.UserNameSearchString))
                {
                    addCommit = false;
                }

                if (addCommit)
                {
                    var commit = new CommitInfo()
                    {
                        Hash = values[0],
                        UserName = values[1],
                        Date = values[2],
                        Message = values[3]
                    };
                    commits.Add(commit);
                }

                if (commits.Count >= props.MaxCommitHistoryCount)
                {
                    break;
                }
            }

            proc.Close();

            return commits;
        }
    }
}
