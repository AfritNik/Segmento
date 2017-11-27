namespace Segmento.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TwitterReportLikesByPeriods",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LikesMedian = c.Double(),
                        BestTimePeriod = c.Int(),
                        UserName = c.String(),
                        LastUpdateDate = c.DateTime(nullable: false),
                        TweetsCount = c.Int(nullable: false),
                        ReportAuthorID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TwitterReportPartLikesByHours",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MainReportID = c.Int(nullable: false),
                        Hour = c.Int(nullable: false),
                        LikesCount = c.Int(nullable: false),
                        LikesMedian = c.Double(),
                        UserName = c.String(),
                        LastUpdateDate = c.DateTime(nullable: false),
                        TweetsCount = c.Int(nullable: false),
                        ReportAuthorID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TwitterReportLikesByPeriods", t => t.MainReportID, cascadeDelete: true)
                .Index(t => t.MainReportID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TwitterReportPartLikesByHours", "MainReportID", "dbo.TwitterReportLikesByPeriods");
            DropIndex("dbo.TwitterReportPartLikesByHours", new[] { "MainReportID" });
            DropTable("dbo.TwitterReportPartLikesByHours");
            DropTable("dbo.TwitterReportLikesByPeriods");
        }
    }
}
