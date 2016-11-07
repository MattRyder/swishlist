namespace Swishlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwnerToWishlist : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Wishlists", name: "ApplicationUser_Id", newName: "Owner_Id");
            RenameIndex(table: "dbo.Wishlists", name: "IX_ApplicationUser_Id", newName: "IX_Owner_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Wishlists", name: "IX_Owner_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Wishlists", name: "Owner_Id", newName: "ApplicationUser_Id");
        }
    }
}
