using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Swishlist.Models
{
    public class Wishlist
    {
        public int ID { get; set; }

        [Display(Name = "Wishlist URL")]
        public string WishlistToken { get; set; }

        [Display(Name = "Wishlist Name")]
        public string Name { get; set; }

        [Display(Name = "Wishlist Description")]
        public string Description { get; set; }

        public string UserID { get; set; }
        
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<WishlistItem> Items { get; set; }

        public WishlistItem GetItem(int wishlistItemID)
        {
            return Items.First(x => x.ID == wishlistItemID);
        }

        public Wishlist()
        {
            Items = new List<WishlistItem>();
        }
    }
}
