namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFOrdemVendaNaturezaOperacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "NaturezaOperacao", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.NotaFiscal", "SefazId", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.OrdemVenda", "NaturezaOperacao", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "NaturezaOperacao");
            DropColumn("dbo.NotaFiscal", "SefazId");
            DropColumn("dbo.NotaFiscal", "NaturezaOperacao");
        }
    }
}
