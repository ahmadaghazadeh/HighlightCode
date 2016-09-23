using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace HighlightCode.App_Start
{
    public static class Extension
    {
        private static readonly string highlightPath = "highlight\\";
        private static readonly string Path = HttpContext.Current.Server.MapPath("~");
        private static readonly string PathHighlight = Path + highlightPath;
        private static readonly string Drive = System.IO.Path.GetPathRoot(Path);
        private static readonly string[] classHtml={"hl",
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


        public static string ToHighLightFormaAndroid(this string str, string lng)
        {
            WirteCodeToFile(str, lng);
            WirteHighlightFile();
            string htmlSource = File.ReadAllText(PathHighlight + "main.html");

            var result = global::PreMailer.Net.PreMailer.MoveCssInline(htmlSource);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(result.Html);

            var pTags = doc.DocumentNode.Descendants("pre");

            var html = pTags.SingleOrDefault().OuterHtml;
            html = classHtml.Aggregate(html, (current, tag) => current.Replace($"class=\"{tag}\"", string.Empty));
            doc.LoadHtml(html);
            pTags = doc.DocumentNode.Descendants();
            foreach (var tag in pTags)
            {
                var styleAttr= tag.Attributes["style"];
                if (styleAttr != null)
                {
                    var style = styleAttr.Value.Split(';');
                    var newSt = new string[] { };
                    tag.Attributes.RemoveAll();
                    foreach (var st in style)
                    {
                        if (st.Contains("background-color"))
                        {
                            newSt = st.Split(':');
                            if (newSt.Length > 0)
                            {
                                tag.Attributes.Add(newSt[0], newSt[1]); ;
                            }
                        }
                        else if(st.Contains("color"))
                        {
                            newSt = st.Split(':');
                            if (newSt.Length > 0)
                            {
                                tag.Attributes.Add(newSt[0], newSt[1]); ;
                            }
                        }
                       
                    }
                    
                    
                    
                }
                if (tag.Name.Equals("span"))
                {
                    tag.Name = "font";
                }
            }
            return doc.DocumentNode.OuterHtml;
        }
        public static string ToHighLightFormat(this string str, string lng)
        {
            WirteCodeToFile(str, lng);
            WirteHighlightFile();
            return File.ReadAllText(PathHighlight + "main.html");

        }
        static void WirteHighlightFile()
        {
            try
            {
                var createFile = "cd " + Drive + Environment.NewLine;
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
            catch (Exception)
            {
                
             
            }
           

        }
        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Console.WriteLine(outLine.Data);
        }
        static void WirteCodeToFile(string str, string lng)
        {
            try
            {
                File.WriteAllLines(System.IO.Path.Combine(Path + highlightPath, "main" + "." + lng), str.Split('\n'));
            }
            catch (Exception)
            {
                
            }
             
        }

    }
}