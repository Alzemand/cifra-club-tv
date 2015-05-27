using System;

namespace AppStudio.Data
{
    public class YouTubePlaylistResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public YouTubePlaylistSnippet snippet { get; set; }
    }

    public class YouTubePlaylistSnippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public string playlistId { get; set; }
        public int position { get; set; }
        public Resourceid resourceId { get; set; }
    }

    public class Resourceid
    {
        public string kind { get; set; }
        public string videoId { get; set; }
    }
}
