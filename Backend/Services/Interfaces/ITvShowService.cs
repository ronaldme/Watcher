namespace Services.Interfaces
{
    public interface ITvShowService
    {
        void AiringToday();
        void TopRated();
        void New();
        void Search();
        void SearchByActor();
        void SearchById();
    }
}