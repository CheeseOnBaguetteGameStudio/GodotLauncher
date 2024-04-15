using Octokit;
using GithubDownload;

namespace GodotLauncher
{
    internal class Program
    {
        private static readonly Credentials _credentials = new Credentials(File.ReadAllText(@"..\..\..\credentials.txt"));
        private static readonly ProductHeaderValue _productHeader = new ProductHeaderValue("DownloadApp");
        private static readonly OctokitGitHubClient _octokitGitHubClient = new OctokitGitHubClient(_productHeader, _credentials);
        internal static readonly string _userFolderPath = @".\godot\versions";

        public static List<string> godotExe { get; set; } = new List<string>();

        static async Task Main(string[] args)
        {
            for (int i = 1; i <= 4; i++)
            {
                await MassDownloadAndUnzip($"4.0.{i}-stable");
                Console.ReadLine();

            }

            Console.ReadLine();
        }

        private static async Task MassDownloadAndUnzip(string tag)
        {
            FilesManager filesManager = new FilesManager(_userFolderPath);

            Release? release = await _octokitGitHubClient.GetRelease("godotengine", "godot", tag);
            if (release == null)
            {
                Console.WriteLine("No tag found");
                return;
            }
            var asset = _octokitGitHubClient.GetAssetsByName("stable_mono_win64.zip", release);

            if (asset != null)
            {
                Console.WriteLine($"NAME: {asset.Name}");
                Console.WriteLine($"ID:   {asset.Id}");
                Console.WriteLine($"URL:  {asset.BrowserDownloadUrl}");
                await _octokitGitHubClient.DownloadAsset(asset, @$".\godot\downloadedFiles");
                Console.WriteLine("Downloaded !");

                Exception? err = filesManager.DecompressFile(@$".\godot\downloadedFiles\{asset.Name}");
                if (err != null) { Console.WriteLine(err.Message); }
            }

            godotExe = filesManager.GetGodotExe();
            //Console.WriteLine("PATHS: ");
            //foreach (string path in godotExe)
            //{

            //    Console.WriteLine(path);
            //}
        }
    }
}