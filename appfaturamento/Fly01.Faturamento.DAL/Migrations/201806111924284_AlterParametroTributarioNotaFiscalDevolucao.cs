namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterParametroTributarioNotaFiscalDevolucao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "ChaveNFeReferenciada", c => c.String(maxLength: 44, unicode: false));
            AddColumn("dbo.OrdemVenda", "ChaveNFeReferenciada", c => c.String(maxLength: 44, unicode: false));
            AlterColumn("dbo.NotaFiscal", "SefazId", c => c.String(maxLength: 44, unicode: false));
            AlterColumn("dbo.ParametroTributario", "MensagemPadraoNota", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ParametroTributario", "MensagemPadraoNota", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.NotaFiscal", "SefazId", c => c.String(maxLength: 50, unicode: false));
            DropColumn("dbo.OrdemVenda", "ChaveNFeReferenciada");
            DropColumn("dbo.NotaFiscal", "ChaveNFeReferenciada");
        }
    }
}
