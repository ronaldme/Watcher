namespace Services.Interfaces
{
    public interface ISubscribe
    {
        /// <summary>
        /// Subscribes on a TV show
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void SubscribeTv(int id);

        /// <summary>
        /// Subscribes on a Movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void SubscribeMovie(int id);
    }
}