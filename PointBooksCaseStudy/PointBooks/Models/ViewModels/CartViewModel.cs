namespace PointBooks.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
        public int CartItemCount => CartItems?.Count ?? 0;
    }
}
