namespace Swishlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameAndDescriptionToWishlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wishlists", "Name", c => c.String());
            AddColumn("dbo.Wishlists", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Wishlists", "Description");
            DropColumn("dbo.Wishlists", "Name");
        }
    }
}
