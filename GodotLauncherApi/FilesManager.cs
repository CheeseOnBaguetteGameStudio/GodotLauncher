using System.IO.Compression;

namespace GodotLauncherApi
{
    public class FilesManager()
    {
        // public string godotFilesPath { get; set; } = Path.GetFullPath(godotFilesPath);


        /// <summary>
        /// Decompress the specified file using <see cref="ZipFile"/>
        /// in <see cref="godotFilesPath"/> path
        /// </summary>
        /// <param name="filename">Path to the file to decompress</param>
        /// <returns>Return an <see cref="Exception"/> if an error occurred, else <see cref="null"/></returns>
        public static Exception? DecompressFile(string fileIn, string folderOut)
        {
            // Normalizes the path.
            folderOut = Path.GetFullPath(folderOut);

            // Create extraction path if it doesn't already exist
            Directory.CreateDirectory(folderOut);

            // Extract to the path
            try
            {
                ZipFile.ExtractToDirectory(fileIn, folderOut);
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
        /// <param name="godotFolder">Folder where to scan</param>
        /// <returns>Return a <see cref="List"/> of all the executable in the <paramref name="godotFolder"/></returns>
        public static List<string> GetGodotExe(string godotFolder)
        {
            List<string> names = new List<string>();
            foreach (string name in Directory.EnumerateDirectories(godotFolder))
            {
                names = names.Append(@$"{name}\{name}.exe").ToList();
            }

            return names;
        }
    }
}
