# SourceInspector
The goal of this program is to group files that are in the same check-in and find correlations.

/**
     *   The goal of this program is to find correlations between files based on a specific feature or bug fixes.
     *   It's not to find dependencies between files or a runtime call graph.
     *   The idea is that for any given feature a group of files will be touched and based on
     *   this to find the most common groupings.  For example, if a change is made to a given 
     *   file A, there is a strong correlation that changes will be made in C, D, and E.
*/

// Set the following properties to customize the program
ToolProperties props = new ToolProperties()
            {
                CommandPath = @"c:\Program Files\Git\bin\git.exe",     // path to your git executable
                //WorkingDirectory = @"c:\Users\dstover\source\reference-repos\control-tool-driver-converter",
                //WorkingDirectory = @"c:\Users\dstover\source\reference-repos\control-app-gcpro",
                WorkingDirectory = @"c:\Users\dstover\source\reference-repos\control-net-sdk",   // path to the repo you are inspecting
                MaxGroupingCount = 8,     // Only groups within min and max range will be shown.
                MinGroupingCount = 1,
                MaxCommitHistoryCount = 500,   // How many commits to inspect
                ShowCommitHash = true,   // Show the hash value of the commit in the output
                OutputFile = "c:/temp/gitoutput.txt",  // The path to the output file
                IgnoreAssemblyInfo = true,  // Ignore changes to AssemblyInfo.cs files
                OnlyShowSourceFiles = true,  // Only show .cs, .xaml, .py files.  Also include .resx files
                ShowMessage = true,  // Show the commit message in the output
                ShowUserName = false,  // Show the user name or email in the output
                UserNameSearchString = "*",  // A string to match the user name or email to use as a filter
                ShowDate = false,  // Show the commit date in the ouptut
                UseRelativeFileNames = true,  // Show file names without any relative path
                OutputEdgeList = false   //  Print the files changed in a single commit as a space delimited list of strings on a single line
                                         // The output can be used as input to a social network graph program such as https://socnetv.org/
            };
