using BLL.Json.Shows;

namespace BLL
{
    public class CurrentNextSeason
    {
        public Messages.DTO.Season Current { get; set; }
        public Messages.DTO.Season Next { get; set; }
    }
}
