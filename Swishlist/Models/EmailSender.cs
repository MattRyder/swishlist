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

        public void SendYouReservedAnItemEmail(WishlistItem wishlistItem)
        {
            MailMessage message = new MailMessage(
               new MailAddress("noreply@swishlist.co", "Swishlist"),
               new MailAddress(wishlistItem.ReservingUser.Email, wishlistItem.ReservingUser.UserName)
           );

            message.Subject = "A Reserved Item has been removed from a Wishlist";
            message.Body = "Hello World";

            emailClient.Send(message);
        }

        public void SendItemRemovedEmail(WishlistItem wishlistItem)
        {
            MailMessage message = new MailMessage(
                new MailAddress("noreply@swishlist.co", "Swishlist"),
                new MailAddress(wishlistItem.ReservingUser.Email, wishlistItem.ReservingUser.UserName)
            );

            message.Subject = "A Reserved Item has been removed from a Wishlist";
            message.Body = "Hello World";

            emailClient.Send(message);
        }
    }
}