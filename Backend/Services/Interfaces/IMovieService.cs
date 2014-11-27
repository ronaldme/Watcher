namespace Services.Interfaces
{
    public interface IMovieService
    {
        void Upcoming();
        void Search();
        void SearchByActor();
        void SearchById();
    }
}
