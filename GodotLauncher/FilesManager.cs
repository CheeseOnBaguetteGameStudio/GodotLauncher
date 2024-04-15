using System.IO.Compression;

namespace GithubDownload
{
    internal class FilesManager
    {
        public string godotFilesPath {  get; set; }

        public FilesManager(string godotFilesPath)
        {
            this.godotFilesPath = Path.GetFullPath(godotFilesPath);
        }

        /// <summary>
        /// Decompress the specified file using <see cref="ZipFile"/>
        /// in <see cref="godotFilesPath"/> path
        /// </summary>
        /// <param name="filename">Path to the file to decompress</param>
        /// <returns></returns>
        public Exception? DecompressFile(string filename)
        {
            string extractPath = this.godotFilesPath;

            // Create extraction path if it doesn't already exist
            Directory.CreateDirectory(extractPath);

            // Normalizes the path.
            extractPath = Path.GetFullPath(extractPath);

            // Extract to the path
            try
            {
                ZipFile.ExtractToDirectory(filename, extractPath);
            }
            catch (Exception e)
            {
                return e;
            }

            return null;

        }

        /// <summary>
        /// Get all the godot exe in the path specified in <see cref="godotFilesPath"/>
        /// </summary>
        public List<string> GetGodotExe()
        {
            List<string> names = new List<string>();
            foreach (string name in Directory.EnumerateDirectories(this.godotFilesPath))
            {
                names = names.Append(@$"{name}\{name}.exe").ToList();
            }

            return names;
        }
    }
}
