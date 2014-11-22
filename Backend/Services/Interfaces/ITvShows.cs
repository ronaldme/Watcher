using System.Collections.Generic;
using Messages.DTO;

namespace Services
{
    public interface ITvShows
    {
        List<TvShowDTO> AiringToday();
        void TopRated();
        List<TvShowDTO> New(int ageInWeeks);
    }
}