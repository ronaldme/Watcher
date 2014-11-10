using System.Collections.Generic;
using Models;

namespace Services
{
    public interface ISearchTv
    {
        List<Show> Search(string input);
        List<Show> SearchByActor(string actorName);
        Show SearchById(int id);
    }
}