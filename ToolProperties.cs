using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceInspector
{
    public class ToolProperties
    {
        public string CommandPath { get; set; }
        public string WorkingDirectory { get; set; }
        public int MaxGroupingCount { get; set; }
        public int MinGroupingCount { get; set; }
        public int MaxCommitHistoryCount { get; set; }
        public bool ShowCommitHash { get; set; }
        public string OutputFile { get; set; }
        public bool IgnoreAssemblyInfo { get; set; }
        public bool OnlyShowSourceFiles { get; set; }
        public bool ShowMessage { get; set; }
        public bool ShowUserName { get; set; }
        public string UserNameSearchString { get; set; } = "*";
        public bool ShowDate { get; set; }
        public bool UseRelativeFileNames { get; set; }
        public bool OutputEdgeList { get; set; }
    }
}
