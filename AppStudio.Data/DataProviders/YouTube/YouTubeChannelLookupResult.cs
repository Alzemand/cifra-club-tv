using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Data
{
    public class YouTubeChannelLookupResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Contentdetails contentDetails { get; set; }
    }

    public class Contentdetails
    {
        public Relatedplaylists relatedPlaylists { get; set; }
    }

    public class Relatedplaylists
    {
        public string favorites { get; set; }
        public string uploads { get; set; }
    }
}
