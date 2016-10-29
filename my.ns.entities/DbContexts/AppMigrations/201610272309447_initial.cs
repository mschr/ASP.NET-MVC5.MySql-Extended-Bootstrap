namespace my.ns.entities.DbContexts.AppMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Thread = c.String(maxLength: 255, storeType: "nvarchar"),
                        Level = c.String(maxLength: 20, storeType: "nvarchar"),
                        Logger = c.String(maxLength: 255, storeType: "nvarchar"),
                        Message = c.String(maxLength: 4000, storeType: "nvarchar"),
                        Exception = c.String(maxLength: 2000, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.logs");
        }
    }
}
