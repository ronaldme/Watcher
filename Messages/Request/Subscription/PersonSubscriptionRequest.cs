namespace Messages.Request
{
    public class PersonSubscriptionRequest
    {
        public string Email { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}