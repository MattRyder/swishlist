namespace Swishlist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCachedSlugToWishlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wishlists", "CachedSlug", c => c.String(nullable: false, maxLength: 250));
            CreateIndex("dbo.Wishlists", "CachedSlug", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Wishlists", new[] { "CachedSlug" });
            DropColumn("dbo.Wishlists", "CachedSlug");
        }
    }
}
