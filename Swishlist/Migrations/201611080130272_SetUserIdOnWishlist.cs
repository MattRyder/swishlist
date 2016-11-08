namespace Swishlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetUserIdOnWishlist : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Wishlists", name: "ApplicationUser_Id", newName: "UserID");
            RenameIndex(table: "dbo.Wishlists", name: "IX_ApplicationUser_Id", newName: "IX_UserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Wishlists", name: "IX_UserID", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Wishlists", name: "UserID", newName: "ApplicationUser_Id");
        }
    }
}
