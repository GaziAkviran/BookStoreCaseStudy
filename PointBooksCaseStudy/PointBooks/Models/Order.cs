﻿namespace PointBooks.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID  { get; set; }
        public int CartID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
