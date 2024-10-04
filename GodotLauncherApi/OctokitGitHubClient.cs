using Octokit;

namespace GodotLauncherApi
{
    public class OctokitGitHubClient : GitHubClient
    {
        public GitHubClient githubClient { get; private set; }
        public static readonly Dictionary<string, string> _paths =
            new Dictionary<string, string>();

        public OctokitGitHubClient(ProductHeaderValue productHeader, Credentials credentials) : base(productHeader)
        {
            githubClient = new GitHubClient(productHeader);
            githubClient.Credentials = credentials;
            base.Credentials = credentials;
        }

        /// <summary>
        /// Get a specific <see cref="Release"/> from the specified <c>Repository</c>.
        /// If not found, return <c>null</c>
        /// </summary>
        /// <param name="repoOwner">Repository owner's name</param>
        /// <param name="repoName">Repository's name</param>
        /// <param name="releaseTagName">Release tag to search for</param>
        /// <returns>Return the last 100 <see cref="Release"/></returns>
        public async Task<Release?> GetRelease(string repoOwner, string repoName, string releaseTagName)
        {
            IReadOnlyList<Release> releases = await this.githubClient.Repository.Release.GetAll(repoOwner, repoName);
            foreach (Release release in releases)
            {
                if (release.TagName == releaseTagName)
                {
                    return release;
                }
            }

            return null;
        }

        /// <summary>
        /// Get all the <see cref="ReleaseAsset"/> of the latest Release
        /// </summary>
        /// <param name="owner">Name of the repository's owner</param>
        /// <param name="name">Name of the repository</param>
        /// <returns>Return the last <see cref="Release"/></returns>
        public async Task<Release?> GetLatestRelease(string owner, string name)
        {
            Release? latestReleases = await this.githubClient.Repository.Release.GetLatest(owner, name);
            return latestReleases;
        }

        /// <summary>
        /// Search for a specific <see cref="ReleaseAsset"/>
        /// </summary>
        /// <param name="name">Asset's name</param>
        /// <param name="release">Release to search in</param>
        /// <returns>Return the <see cref="ReleaseAsset"/> obtained in the <see cref="Release"/></returns>
        public static ReleaseAsset? GetAssetsByName(string name, Release release)
        {
            var latestAssets = release.Assets;
            foreach (ReleaseAsset asset in latestAssets)
            {
                if (asset.Name.EndsWith(name))
                {
                    return asset;
                }
            }

            return null;
        }

        /// <summary>
        /// Download the specified <see cref="ReleaseAsset"/> in the passed path using <see cref="HttpClient"/>
        /// </summary>
        /// <param name="asset"><see cref="ReleaseAsset"/> to download</param>
        /// <param name="path">Path where to download the <see cref="ReleaseAsset"/></param>
        /// <returns><see cref="void"/></returns>
        public static async Task DownloadAsset(ReleaseAsset asset, string path)
        {
            string url = asset.BrowserDownloadUrl;

            byte[] content;
            using (HttpClient client = new HttpClient())
            {
                content = await client.GetByteArrayAsync(url);
            }

            Directory.CreateDirectory(path);
            string dest = @$"{path}\{asset.Name}";
            File.WriteAllBytes(dest, content);
        }

    }
}
