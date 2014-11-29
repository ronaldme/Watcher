namespace Services.Interfaces
{
    public interface IMovieService
    {
        void Upcoming();
        void Search();
        void SearchByPerson();
        void SearchById();
    }
}
