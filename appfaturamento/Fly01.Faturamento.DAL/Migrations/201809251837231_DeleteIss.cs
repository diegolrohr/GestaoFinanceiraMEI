namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteIss : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM ISS");
        }
        
        public override void Down()
        {
        }
    }
}
