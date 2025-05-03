namespace Scamazon.Offers
{
    public class Rating
    {
        public int NumOfReviews { get; set; }
        public int Stars { get; set; }
        public Review[] Reviews { get; set; }
    }
}
