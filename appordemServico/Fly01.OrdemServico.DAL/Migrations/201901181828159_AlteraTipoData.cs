namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteraTipoData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrdemServico", "DataEmissao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrdemServico", "DataEntrega", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdemServico", "DataEntrega", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.OrdemServico", "DataEmissao", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
