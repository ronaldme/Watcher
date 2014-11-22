using System.Collections.Generic;
using Models;

namespace Services
{
    public interface ITvShows
    {
        List<Show> AiringToday();
        List<Show> TopRated();
        List<Show> New(int ageInWeeks);
    }
}