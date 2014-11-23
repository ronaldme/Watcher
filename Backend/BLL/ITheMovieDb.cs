using System.Collections.Generic;
using Messages.DTO;

namespace BLL
{
    public interface ITheMovieDb
    {
        List<TvShowDTO> SearchTv(string search);
        List<TvShowDTO> GetTopRated();
        TvShowDTO GetBy(int id);
    }
}