using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private static readonly string[] classHtml={
            "hl num",
"hl esc",
"hl str",
"hl pps",
"hl slc",
"hl com",
"hl ppc",
"hl opt",
"hl ipl",
"hl lin",
"hl kwa",
"hl kwb",
"hl kwc",
"hl kwd"};


        public static string ToHighLightFormat(this string str, string lng)
        {
            WirteCodeToFile(str, lng);
            WirteHighlightFile();
            string htmlSource = File.ReadAllText(PathHighlight + "main.html");

            var result = global::PreMailer.Net.PreMailer.MoveCssInline(htmlSource);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(result.Html);
            
            var pTags = doc.DocumentNode.Descendants("pre");
            var html = pTags.SingleOrDefault().OuterHtml;
            html = classHtml.Aggregate(html, (current, tag) => current.Replace("class=" + tag, ""));

            return html;

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