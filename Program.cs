using System;
using System.IO;
using System.Linq;
using EmbedIO;
using Swan.Logging;

namespace SimpleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseUrl = "http://localhost:9696/";
            var fileRootArg = args.ElementAtOrDefault(0);// ?? throw new ArgumentException("Missing first command line argument! The root directory for the static file server must be a valid path in the file system!");

            if (string.IsNullOrWhiteSpace(fileRootArg))
            {
                WriteRedLine("\nExecution terminated! Missing first command line argument!");
                WriteRedLine("The root directory for the static file server must be a valid path in the file system!");
                return;
            }

            string fileRoot, defaultFile;
            try
            {
                fileRoot = GetAbsolutePath(fileRootArg);
                defaultFile = GetDefaultFile(fileRoot);
                
            }
            catch (System.Exception ex)
            {
                WriteRedLine(ex.Message);
                return;
            }

            using (var server = CreateWebServer(baseUrl, fileRoot))
            {
                server.RunAsync();

                server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();
                $"StaticFileServer running on directory {fileRoot}".Info();

                var startingUrl = baseUrl + defaultFile;

                var browser = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo(startingUrl) { UseShellExecute = true }
                };

                $"Starting page for web content: '{startingUrl}'".Info();

                browser.Start();

                Console.ReadKey();
            }

        }

        private static WebServer CreateWebServer(string url, string fileRoot)
        {
            return new WebServer(o => o.WithUrlPrefix(url)).WithStaticFolder("/", fileRoot, true);
        }
        
        private static string GetDefaultFile(string directoryPath){
            var validDefaultFiles = new string[]{"index.html","index.htm","default.html","default.htm"};
            var foundDefaultFile =  Directory.GetFiles(directoryPath).FirstOrDefault(f=> validDefaultFiles.Contains(new FileInfo(f).Name));

            if (foundDefaultFile == null)
                throw new ArgumentException($"{directoryPath} does not contain any of the defaultfiles '[{string.Join(',',validDefaultFiles)}]'");

            return new FileInfo(foundDefaultFile).Name;
        }

        private static void WriteRedLine(string errorText)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(errorText);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static string GetAbsolutePath(string path){

            string fileRoot = string.Empty;

            try
            {
                System.Console.WriteLine($"Trying to get absolute path for: '{path}'");
                fileRoot = Path.GetFullPath(path);
            }
            catch(Exception)
            {
                // WriteRedLine($"\nThe provided path '{fileRootArg}' is not a valid path!");
                throw new ArgumentException($"\nThe provided path '{path}' is not a valid path!");
            }

            if (!Directory.Exists(fileRoot)){
                // WriteRedLine($"\n'{fileRoot}' is not a valid path!");
                throw new DirectoryNotFoundException($"\n'{fileRoot}' is not a valid path!");
            }

            return fileRoot;
        }
    }
}
