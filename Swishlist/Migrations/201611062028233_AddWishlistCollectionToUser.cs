namespace Swishlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWishlistCollectionToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wishlists", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Wishlists", "ApplicationUser_Id");
            AddForeignKey("dbo.Wishlists", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wishlists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Wishlists", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Wishlists", "ApplicationUser_Id");
        }
    }
}
