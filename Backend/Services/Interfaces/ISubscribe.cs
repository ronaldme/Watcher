namespace Services.Interfaces
{
    public interface ISubscribe
    {
        /// <summary>
        /// Subscribe on a TV show
        /// </summary>
        void SubscribeTv();

        /// <summary>
        /// Subscribes on a Movie
        /// </summary>
        void SubscribeMovie();

        /// <summary>
        /// Subscribe on a Actor/Actress
        /// </summary>
        void SubscribeActor();
    }
}