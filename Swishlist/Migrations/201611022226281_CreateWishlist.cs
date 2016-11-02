namespace Swishlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateWishlist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wishlists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WishlistToken = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.WishlistItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        Description = c.String(),
                        URL = c.String(),
                        ReservingUser_Id = c.String(maxLength: 128),
                        Wishlist_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ReservingUser_Id)
                .ForeignKey("dbo.Wishlists", t => t.Wishlist_ID)
                .Index(t => t.ReservingUser_Id)
                .Index(t => t.Wishlist_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishlistItems", "Wishlist_ID", "dbo.Wishlists");
            DropForeignKey("dbo.WishlistItems", "ReservingUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.WishlistItems", new[] { "Wishlist_ID" });
            DropIndex("dbo.WishlistItems", new[] { "ReservingUser_Id" });
            DropTable("dbo.WishlistItems");
            DropTable("dbo.Wishlists");
        }
    }
}
