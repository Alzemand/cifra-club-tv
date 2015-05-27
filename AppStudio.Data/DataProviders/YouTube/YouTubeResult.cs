using System.Collections.Generic;

namespace AppStudio.Data
{
    public class YouTubeResult<T>
    {
        public string error { get; set; }
        public List<T> items { get; set; }
    }
}
