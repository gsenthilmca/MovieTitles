using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MovieTitles
{
    public class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string imdbID { get; set; }
    }
    public class MovieSearch
    {
        public string page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<Movie> data { get; set; }
    }
    public class Solution
    {
        public static void Main(string[] args)
        {
            var title = Console.ReadLine();
            string[] movieTtiles = getMovieTitles(title);
            foreach(var name in movieTtiles)
            {
                Console.WriteLine(name);
            }
            Console.ReadLine();
        }

        private static string[] getMovieTitles(string title)
        {
            string movieSearchEndpoint = "https://jsonmock.hackerrank.com/api/movies/search/?Title={0}&page={1}";
            List<string> movieTitles = new List<string>();
            var data = HttpGet(string.Format(movieSearchEndpoint, title, "1"));
            var searchResults = JsonConvert.DeserializeObject<MovieSearch>(data);

            AddToMovieTitles(searchResults, ref movieTitles);

            while (Convert.ToInt32(searchResults.page) < searchResults.total_pages)
            {
                data = HttpGet(string.Format(movieSearchEndpoint, title, Convert.ToInt32(searchResults.page) + 1));
                searchResults = JsonConvert.DeserializeObject<MovieSearch>(data);
                AddToMovieTitles(searchResults,ref  movieTitles);
            }
            return movieTitles.OrderBy(t=>t).ToArray();
        }

        private static void AddToMovieTitles(MovieSearch data, ref List<string> movieTitles)
        {
            foreach(var movie in data.data)
            {
                movieTitles.Add(movie?.Title);
            }
        }
        private static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string data;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                data = reader.ReadToEnd();
            }
            return data;
        }
    }
}
