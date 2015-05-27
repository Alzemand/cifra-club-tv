using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace AppStudio.Data
{
    public class YouTubeDataProvider
    {
        private const string _baselUrl = @"https://www.googleapis.com/youtube/v3";
        private YouTubeDataType _queryType;
        private string _query;
        private string _apiKey;

        public YouTubeDataProvider(YouTubeDataType queryType, string query, OAuthTokens tokens)
        {
            this._queryType = queryType;
            this._query = query;
            this._apiKey = tokens["ApiKey"];
        }

        private string ChannelUrl
        {
            get
            {
                return string.Format("{0}/channels?forUsername={1}&part=contentDetails&maxResults=1&key={2}", _baselUrl, _query, _apiKey);
            }
        }

        private string PlaylistUrl(string playlistId)
        {
            return string.Format("{0}/playlistItems?playlistId={1}&part=snippet&maxResults=20&key={2}", _baselUrl, playlistId, _apiKey);
        }

        private string SearchUrl
        {
            get
            {
                return string.Format("{0}/search?q={1}&part=snippet&maxResults=20&key={2}&type=video", _baselUrl, _query, _apiKey);
            }
        }

        public async Task<IEnumerable<YouTubeSchema>> LoadAsync()
        {
            try
            {
                switch (_queryType)
                {
                    case YouTubeDataType.Channels:
                        return await LoadChannelAsync();
                    case YouTubeDataType.Videos:
                        return await SearchAsync();
                    case YouTubeDataType.Playlist:
                        return await LoadPlaylistAsync(_query);
                    default:
                        return new ObservableCollection<YouTubeSchema>();
                }
            }
            catch (WebException wex)
            {
                return GenerateErrorYouTube(HandleException(wex.Response));
            }
        }

        private static string HandleException(WebResponse wResponse)
        {
            string responseText;

            using (StreamReader sr = new StreamReader(wResponse.GetResponseStream()))
            {
                responseText = sr.ReadToEnd();
            }

            if (responseText.Contains("playlistNotFound"))
            {
                return "Playlist not found";
            }
            if (responseText.Contains("accessNotConfigured"))
            {
                return "The API Key is no longer active";
            }
            if (responseText.Contains("keyExpired"))
            {
                return "The API Key expired";
            }
            if (responseText.Contains("keyInvalid"))
            {
                return "The API Key is invalid";
            }
            return "Error accessing YouTube";
        }


        private async Task<IEnumerable<YouTubeSchema>> LoadChannelAsync()
        {
            IEnumerable<YouTubeSchema> result = new ObservableCollection<YouTubeSchema>();
            var listId = await GetUploadVideosListId();
            if (!string.IsNullOrEmpty(listId))
            {
                result = await LoadPlaylistAsync(listId);
            }
            return result;
        }

        private async Task<IEnumerable<YouTubeSchema>> SearchAsync()
        {
            string searchResult = await DownloadAsync(SearchUrl);
            var searchList = JsonConvert.DeserializeObject<YouTubeResult<YouTubeSearchResult>>(searchResult);
            if (searchList != null && searchList.items != null)
            {
                ObservableCollection<YouTubeSchema> resultToReturn = new ObservableCollection<YouTubeSchema>();
                foreach (var i in searchList.items)
                {
                    var item = new YouTubeSchema()
                    {
                        Title = i.snippet.title,
                        ImageUrl = i.snippet.thumbnails != null ? i.snippet.thumbnails.high.url : string.Empty,
                        Summary = i.snippet.description,
                        Published = i.snippet.publishedAt,
                        VideoId = i.id.videoId
                    };
                    resultToReturn.Add(item);
                }
                return resultToReturn;
            }
            return new ObservableCollection<YouTubeSchema>();
        }

        public async Task<string> GetUploadVideosListId()
        {
            string channelSearchResult = await DownloadAsync(ChannelUrl);
            var channel = JsonConvert.DeserializeObject<YouTubeResult<YouTubeChannelLookupResult>>(channelSearchResult);

            if (channel != null
                && channel.items != null
                && channel.items.Count > 0
                && channel.items[0].contentDetails != null
                && channel.items[0].contentDetails.relatedPlaylists != null
                && !string.IsNullOrEmpty(channel.items[0].contentDetails.relatedPlaylists.uploads))
            {
                return channel.items[0].contentDetails.relatedPlaylists.uploads;
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task<IEnumerable<YouTubeSchema>> LoadPlaylistAsync(string playlistId)
        {
            string playlistUrl = PlaylistUrl(playlistId);
            string playlistResult = await DownloadAsync(playlistUrl);
            var playlist = JsonConvert.DeserializeObject<YouTubeResult<YouTubePlaylistResult>>(playlistResult);
            if (playlist != null && playlist.items != null)
            {
                return playlist.items
                    .Select(i => new YouTubeSchema()
                    {
                        Title = i.snippet.title,
                        ImageUrl = i.snippet.thumbnails != null ? i.snippet.thumbnails.high.url : string.Empty,
                        Summary = i.snippet.description,
                        Published = i.snippet.publishedAt,
                        VideoId = i.snippet.resourceId.videoId
                    }).ToList();
            }
            return new ObservableCollection<YouTubeSchema>();
        }

        private async Task<string> DownloadAsync(string url)
        {
            WebRequest request = WebRequest.CreateHttp(url);
            request.UseDefaultCredentials = true;
            request.Method = "GET";
            var response = await request.GetResponseAsync();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
        private static IEnumerable<YouTubeSchema> GenerateErrorYouTube(string text)
        {
            return new YouTubeSchema[]
            {
                new YouTubeSchema()
                {
                    VideoId = string.Empty,
                    Title = text,
                    Summary = text,
                    ImageUrl = "ms-appx:///Assets/ErrorImage.png"                    
                }
            };
        }
    }
}