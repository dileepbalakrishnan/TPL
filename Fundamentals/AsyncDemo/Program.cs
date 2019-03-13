using System;
using System.Net;
using System.Threading.Tasks;

namespace AsyncDemo
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var title1 = GetTitle("https://www.ndtv.com/");
            Console.WriteLine($"Title using synchronous call : {title1}");
            var title2 = GetTitleTplAsync("https://www.ndtv.com/").Result;
            Console.WriteLine($"Title using TPL Async : {title2}");
            var title3 = GetTitleCsAsync("https://www.ndtv.com/").Result;
            Console.WriteLine($"Title using async : {title3}");
            Console.ReadLine();
        }

        private static string GetTitle(string url)
        {
            using (var wc = new WebClient())
            {
                var content = wc.DownloadString(url);
                return ExtractTitle(content);
            }
        }

        private static Task<string> GetTitleTplAsync(string url)
        {
            var wc = new WebClient();
            var downloadTask = wc.DownloadStringTaskAsync(url);
            return downloadTask.ContinueWith(task =>
            {
                wc.Dispose();
                var title = ExtractTitle(task.Result);
                return title;
            });
        }

        private static async Task<string> GetTitleCsAsync(string url)
        {
            using (var wc = new WebClient())
            {
                var content = await wc.DownloadStringTaskAsync(url);
                return ExtractTitle(content);
            }
        }

        private static string ExtractTitle(string content)
        {
            var titleStart = content.IndexOf("<title>", StringComparison.OrdinalIgnoreCase) + 7;
            var titleEnd = content.IndexOf("</title>", StringComparison.OrdinalIgnoreCase);
            var title = content.Substring(titleStart, titleEnd - titleStart);
            return title;
        }
    }
}