using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceInspector
{
    /**
     *   The goal of this program is to find correlations between files based on a specific feature or bug fixes.
     *   It's not to find dependencies between files or a runtime call graph.
     *   The idea is that for any given feature a group of files will be touched and based on
     *   this to find the most common groupings.  For example, if a change is made to a given 
     *   file A, there is a strong correlation that changes will be made in C, D, and E.
     */
    class Program
    {
        static void Main(string[] args)
        {
            ToolProperties props = new ToolProperties()
            {
                CommandPath = @"c:\Program Files\Git\bin\git.exe",
                //WorkingDirectory = @"c:\Users\dstover\source\reference-repos\control-tool-driver-converter",
                //WorkingDirectory = @"c:\Users\dstover\source\reference-repos\control-app-gcpro",
                WorkingDirectory = @"c:\Users\dstover\source\reference-repos\control-net-sdk",
                MaxGroupingCount = 8,
                MinGroupingCount = 1,
                MaxCommitHistoryCount = 500,
                ShowCommitHash = true,
                OutputFile = "c:/temp/gitoutput.txt",
                IgnoreAssemblyInfo = true,
                OnlyShowSourceFiles = true,
                ShowMessage = true,
                ShowUserName = false,
                UserNameSearchString = "*",
                ShowDate = false,
                UseRelativeFileNames = true,
                OutputEdgeList = false
            };

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            var commits = CommitProvider.GetCommits(props);

            int currentCount = 0;
            foreach (var commit in commits)
            {
                currentCount++;
                Console.WriteLine(commit.Hash + " : " + currentCount + " / " + props.MaxCommitHistoryCount);

                CommitInfoProvider.PopulateFileChanged(commit, props);

                bool includeCommit = true;

                if (commit.FilesChanged.Count < props.MinGroupingCount || commit.FilesChanged.Count > props.MaxGroupingCount)
                {
                    includeCommit = false;
                }

                if (props.UserNameSearchString != "*" && !commit.UserName.Contains(props.UserNameSearchString))
                {
                    includeCommit = false;
                }

                if (includeCommit)
                {
                    if (props.OutputEdgeList)
                    {
                        if (commit.FilesChanged.Count == 0)
                        {
                            continue;
                        }

                        foreach (var file in commit.FilesChanged)
                        {
                            string fileNameOnly = RemoveRelativePathFromFileName(file);
                            writer.Write(fileNameOnly + " ");
                        }
                        writer.WriteLine();
                    }
                    else
                    {
                        if (props.ShowDate)
                        {
                            writer.WriteLine(commit.Date);
                        }
                        if (props.ShowCommitHash)
                        {
                            writer.WriteLine(commit.Hash);
                        }
                        if (props.ShowUserName)
                        {
                            writer.WriteLine(commit.UserName);
                        }
                        if (props.ShowMessage)
                        {
                            string message = commit.Message;
                            int colonPosition = message.IndexOf(":");
                            if (colonPosition > 0)
                            {
                                //string jiraIssue = message.Substring(0, colonPosition - 1);
                                //string title = JiraReader.GetJiraTitle(jiraIssue.Trim());
                                //writer.WriteLine("Jira Title: " + title);
                            }
                            writer.WriteLine(commit.Message);
                        }
                        foreach (var file in commit.FilesChanged)
                        {
                            string name = file;
                            if (props.UseRelativeFileNames)
                            {
                                name = RemoveRelativePathFromFileName(file);
                            }
                            writer.WriteLine(name);
                        }
                        writer.WriteLine();
                    }
                }
            }

            writer.Flush();
            using (FileStream file = new FileStream(props.OutputFile, FileMode.Create))
            {
                stream.Position = 0;
                stream.Read(stream.ToArray(), 0, (int)stream.Length);
                file.Write(stream.ToArray(), 0, stream.ToArray().Length);
                stream.Close();
                file.Flush();
                file.Close();
                writer.Close();
            }
        }

        private static string RemoveRelativePathFromFileName(string name)
        {
            string fileNameOnly = "";
            int slash = name.LastIndexOf('/');
            if (slash > -1)
            {
                fileNameOnly = name.Substring(slash + 1, name.Length - slash - 1);
            }
            return fileNameOnly;
        }
    }
}
