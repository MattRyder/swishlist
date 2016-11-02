using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swishlist.Models
{
    public class WishlistItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public virtual ApplicationUser ReservingUser { get; set; }
    }
}
