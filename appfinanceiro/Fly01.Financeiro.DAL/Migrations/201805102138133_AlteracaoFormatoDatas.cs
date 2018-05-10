namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracaoFormatoDatas : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cnab", "DataEmissao", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Cnab", "DataVencimento", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Cnab", "DataDesconto", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cnab", "DataDesconto", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cnab", "DataVencimento", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cnab", "DataEmissao", c => c.DateTime(nullable: false));
        }
    }
}
