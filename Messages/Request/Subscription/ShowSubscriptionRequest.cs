namespace Messages.Request
{
    public class ShowSubscriptionRequest
    {
        public string Email { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
