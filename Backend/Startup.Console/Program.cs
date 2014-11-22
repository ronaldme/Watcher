using Services;

namespace Startup.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tvShows = new TvShows();
            tvShows.TopRated();

            var searchTv = new SearchTv();
            searchTv.Search();
            searchTv.SearchById();

            System.Console.WriteLine("Services started!");
            System.Console.ReadLine();
        }
    }
}
