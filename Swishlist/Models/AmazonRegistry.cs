using HtmlAgilityPack;
using Nager.AmazonProductAdvertising;
using Nager.AmazonProductAdvertising.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Swishlist.Models
{
    public class AmazonRegistry
    {
        const string AMAZON_REGISTRY_URL_FORMAT = "https://www.amazon.co.uk/gp/registry/wishlist/{0}/ref=cm_wl_list_o_1?";

        private Wishlist wishlist;
        private HtmlDocument pageDocument;
        private AmazonWrapper amazonClient;

        /// <summary>
        /// Create an instance of the AmazonRegistry class with a given Wishlist
        /// </summary>
        /// <param name="wishlist">Wishlist to use to construct the parse request</param>
        public AmazonRegistry(Wishlist wishlist)
        {
            pageDocument = new HtmlWeb().Load(string.Format(AMAZON_REGISTRY_URL_FORMAT, wishlist.WishlistToken));
            this.wishlist = wishlist;

            amazonClient = new AmazonWrapper(
                new AmazonAuthentication()
                {
                    AccessKey = ConfigurationManager.AppSettings["AmazonClientID"],
                    SecretKey = ConfigurationManager.AppSettings["AmazonClientSecret"]
                },
                AmazonEndpoint.UK,
                Environment.GetEnvironmentVariable("AMAZON_AFFILIATE_ID")
            );
        }

        public bool Parse()
        {       
            const string SELECTOR_WISHLIST_ITEM_CONTAINER = "wl-item-view";
            const string AMAZON_ASIN_REGEX_PATTERN = "(?<=/dp/)([0-9A-z]{10})";

            bool dbChangesPending = false;
            List<string> parsedReferences = new List<string>();
            HtmlNode itemPageWrapperNode = pageDocument.GetElementbyId(SELECTOR_WISHLIST_ITEM_CONTAINER);

            if (itemPageWrapperNode == null) return false;

            IEnumerable<HtmlNode> itemElements = itemPageWrapperNode.DescendantsAndSelf("div").Where(x => x.Id.StartsWith("item_"));
            
            foreach(var itemElement in itemElements)
            {
                HtmlNode itemNameNode = itemElement.Descendants("a").FirstOrDefault(x => x.Id.StartsWith("itemName_"));

                string hrefString = itemNameNode.GetAttributeValue("href", "");
                Match asinMatch = Regex.Match(itemNameNode.GetAttributeValue("href", ""), AMAZON_ASIN_REGEX_PATTERN);

                if (!asinMatch.Success) return false;

                parsedReferences.Add(asinMatch.Value);

                // if exists in the item list already, skip (for now):
                if (wishlist.Items.Any(item => item.Reference.Equals(asinMatch.Value))) {
                    continue;
                }

                // Go get the item:
                WishlistItem amazonProductItem = GetItem(asinMatch.Value);
                if(amazonProductItem != null)
                {
                    wishlist.Items.Add(amazonProductItem);
                    dbChangesPending = true;
                }
            }

            // Find any items in the DB that have been dropped from the list:
            IEnumerable<WishlistItem> removedItems = wishlist.Items.Where(item => !parsedReferences.Contains(item.Reference));
            if (removedItems.Count() > 0)
            {
                for(int i = 0; i < removedItems.Count(); i++)
                {
                    WishlistItem item = removedItems.ElementAt(i);
                    if(wishlist.Items.Remove(item) && item.ReservingUser != null)
                        new EmailSender().SendItemRemovedEmail(item);
                    
                    dbChangesPending = true;
                }    
            }
            
            return dbChangesPending;

        }

        /// <summary>
        /// Get the Amazon product listing for an item via cache or fresh via AWS
        /// </summary>
        /// <param name="itemASIN">Amazon ASIN to lookup</param>
        /// <returns>Item if found, or null if nonexistant</returns>
        private WishlistItem GetItem(string itemASIN)
        {
            if (itemASIN == null) return null;

            string cacheKey = string.Format("AmazonRegistry_{0}", itemASIN);
            WishlistItem cachedItem = (WishlistItem)HttpRuntime.Cache[cacheKey];

            if(cachedItem != null)
            {
                Console.WriteLine("AmazonRegistry: Cache Hit");
                return cachedItem;
            } else
            {
                Console.WriteLine("AmazonRegistry: Cache Miss");

                var lookup = amazonClient.Lookup(itemASIN);

                if (lookup != null && lookup.Items.Item.Length > 0)
                {
                    Item productItem = lookup.Items.Item[0];

                    WishlistItem item = new WishlistItem()
                    {
                        Name = productItem.ItemAttributes.Title,
                        URL = productItem.DetailPageURL,
                        Description = productItem.SalesRank,
                        ImageUrl = productItem.LargeImage.URL,
                        Reference = productItem.ASIN
                    };

                    HttpRuntime.Cache[cacheKey] = item;
                    return item;
                }
            }

            return null;
        }
    }
}
