using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SourceInspector
{
    public static class JiraReader
    {
        public static string GetJiraTitle(string jiraIssue)
        {
            string jiraURL = "https://extron.atlassian.net/browse/" + jiraIssue;
            WebClient web = new WebClient();
            web.Credentials = new NetworkCredential("dstover@extron.com", "SoWorkContinues667$");
            string source = web.DownloadString(jiraURL);
            string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
            return title;
        }
    }
}
