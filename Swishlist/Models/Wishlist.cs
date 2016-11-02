using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Swishlist.Models
{
    public class Wishlist
    {
        public int ID { get; set; }

        [Display(Name = "Wishlist URL")]
        public string WishlistToken { get; set; }

        public IList<WishlistItem> Items { get; set; }
    }
}
