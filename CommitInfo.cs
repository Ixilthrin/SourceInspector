using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceInspector
{
    public class CommitInfo
    {
        public string Hash { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public List<string> FilesChanged { get; set; } = new List<string>();
    }
}
