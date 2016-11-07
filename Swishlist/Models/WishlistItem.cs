using System.ComponentModel.DataAnnotations.Schema;

namespace Swishlist.Models
{
    public class WishlistItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }

        public string Reference { get; set; }

        public virtual Wishlist Wishlist { get; set; }
        public virtual ApplicationUser ReservingUser { get; set; }
    }
}
