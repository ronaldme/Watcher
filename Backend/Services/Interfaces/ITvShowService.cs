namespace Services.Interfaces
{
    public interface ITvShowService
    {
        void AiringToday();
        void TopRated();
        void New();
        void Search();
        void SearchByPerson();
        void SearchById();
    }
}