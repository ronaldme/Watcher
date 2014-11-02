namespace Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string AirDate { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
    }
}