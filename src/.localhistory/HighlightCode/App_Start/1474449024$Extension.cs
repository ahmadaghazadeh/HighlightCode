using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace HighlightCode.App_Start
{
    public static class Extension
    {
        private static readonly string highlightPath = "highlight\\";
        private static readonly string Path = HttpContext.Current.Server.MapPath("~");
        private static readonly string PathHighlight = Path + highlightPath;
        private static readonly string Drive = System.IO.Path.GetPathRoot(Path);
        public static string ToHighLightFormat(this string str, string lng)
        {
            WirteCodeToFile(str, lng);
            WirteHighlightFile();
            string htmlSource = File.ReadAllText(PathHighlight + "main.html");

            var result = global::PreMailer.Net.PreMailer.MoveCssInline(htmlSource);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(result.Html);

            var pTags = doc.DocumentNode.Descendants("pre");
            //result.Html;        // Resultant HTML, with CSS in-lined.

            //result.Warnings;     // string[] of any warnings that occurred during processing.
            return File.ReadAllText(PathHighlight + "main.html");

        }
        static void WirteHighlightFile()
        {

            var createFile = "cd "+Drive + Environment.NewLine;
            createFile += "cd " + PathHighlight + Environment.NewLine;
            createFile += "highlight -i main.java -o main.html --style custom --include-style " + Environment.NewLine;

            File.WriteAllText(PathHighlight + "Create.bat", createFile);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = PathHighlight,
                    FileName = PathHighlight + "Create.bat",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false
                }
            };
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

        }
        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Console.WriteLine(outLine.Data);
        }
        static void WirteCodeToFile(string str, string lng)
        {
             File.WriteAllLines(System.IO.Path.Combine(Path+ highlightPath, "main" + "." + lng), str.Split('\n'));
        }

    }
}