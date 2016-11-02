using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Swishlist.Models
{
    public class AmazonRegistry
    {
        private HtmlDocument pageDocument;
        private List<WishlistItem> items;

        public AmazonRegistry()
        {
            items = new List<WishlistItem>();
        }

        public AmazonRegistry(HtmlDocument pageDocument) : this()
        {
            this.pageDocument = pageDocument;
        }

        public bool Parse()
        {
            const string SELECTOR_WISHLIST_ITEM_CONTAINER = "wl-item-view";
            HtmlNode itemPageWrapperNode = pageDocument.GetElementbyId(SELECTOR_WISHLIST_ITEM_CONTAINER);

            if (itemPageWrapperNode == null) return false;

            IEnumerable<HtmlNode> itemElements = itemPageWrapperNode.DescendantsAndSelf("div").Where(x => x.Id.StartsWith("item_"));
            
            foreach(var itemElement in itemElements) {
                HtmlNode itemNameNode = itemElement.Descendants("a").FirstOrDefault(x => x.Id.StartsWith("itemName_"));
                WishlistItem item = new WishlistItem()
                {
                    Name = itemNameNode.GetAttributeValue("title", null),
                    URL = itemNameNode.GetAttributeValue("href", null),
                    Description = itemNameNode.NextSibling.InnerText
                };

                int u = 0;
            }

            itemElements.ToArray();

            return true;

        }
    }
}
