namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParametroTributarioAliquotaCSLLeINSSeIR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "AliquotaCSLL", c => c.Double(nullable: false));
            AddColumn("dbo.ParametroTributario", "AliquotaINSS", c => c.Double(nullable: false));
            AddColumn("dbo.ParametroTributario", "AliquotaImpostoRenda", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "AliquotaImpostoRenda");
            DropColumn("dbo.ParametroTributario", "AliquotaINSS");
            DropColumn("dbo.ParametroTributario", "AliquotaCSLL");
        }
    }
}
