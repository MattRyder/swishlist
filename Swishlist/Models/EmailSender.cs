using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Swishlist.Models
{
    public class EmailSender
    {
        const string FROM_EMAIL_ADDRESS = "noreply@swishlist.co";
        const string FROM_DISPLAY_NAME = "Swishlist";

        SmtpClient emailClient;

        public EmailSender()
        {
            emailClient = new SmtpClient()
            {
                Host = "127.0.0.1",
                Port = 25,
                Credentials = new NetworkCredential()
                {
                    UserName = "username",
                    Password = "password"
                }
            };
        }

        public void SendItemRemovedEmail(WishlistItem wishlistItem)
        {
            MailMessage message = new MailMessage(
                new MailAddress("noreply@swishlist.co", "Swishlist"),
                new MailAddress(wishlistItem.ReservingUser.Email, wishlistItem.ReservingUser.Name)
            );

            message.Subject = "A Reserved Item has been removed from a Wishlist";
            message.Body = string.Format("{0} has been removed from the wishlist by {1}. Oh no!", wishlistItem.Name, wishlistItem.Wishlist.User.Name);

            emailClient.Send(message);
        }
    }
}