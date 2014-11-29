namespace Services.Interfaces
{
    public interface ISubscriptionService
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
        /// Subscribe on a person
        /// </summary>
        void SubscribePerson();

        /// <summary>
        /// Unsubscribe on a TV show
        /// </summary>
        void UnsubscribeTv();

        /// <summary>
        /// Unsubscribe on a Movie
        /// </summary>
        void UnsubscribeMovie();

        /// <summary>
        /// Unsubscribes on a person
        /// </summary>
        void UnsubscribePerson();
    }
}