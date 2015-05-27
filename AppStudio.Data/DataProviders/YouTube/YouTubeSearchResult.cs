using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Data
{
    public class YouTubeSearchResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public YouTubeSearchId id { get; set; }
        public YouTubeSearchSnippet snippet { get; set; }
    }


    public class YouTubeSearchId
    {
        public string kind { get; set; }
        public string videoId { get; set; }
        public string playlistId { get; set; }
    }

    public class YouTubeSearchSnippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public string liveBroadcastContent { get; set; }
    }
}
