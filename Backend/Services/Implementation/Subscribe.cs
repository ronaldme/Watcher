using EasyNetQ;
using Messages.Request;
using Messages.Response;
using Services.Interfaces;

namespace Services
{
    public class Subscribe : ISubscribe, IMqResponder
    {
        private readonly IBus bus;

        public Subscribe(IBus bus)
        {
            this.bus = bus;
        }

        public void SubscribeTv(int id)
        {
            bus.Respond<TvSubscription, Subscription>(request => new Subscription
            {
                
            });
        }

        public void SubscribeMovie(int id)
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
