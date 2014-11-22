namespace Services
{
    public interface ISubscribe
    {
        /// <summary>
        /// Subscribes on a TV show
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SubscribeTv(int id);

        /// <summary>
        /// Subscribes on a Movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SubscribeMovie(int id);
    }
}