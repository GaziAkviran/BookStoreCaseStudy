namespace PointBooks.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }
    }
}
