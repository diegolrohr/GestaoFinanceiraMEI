namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterParametroTributario1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "MensagemPadraoNota", c => c.String(maxLength: 5000, unicode: false));
            AddColumn("dbo.OrdemVenda", "MensagemPadraoNota", c => c.String(maxLength: 5000, unicode: false));
            AlterColumn("dbo.ParametroTributario", "MensagemPadraoNota", c => c.String(maxLength: 5000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ParametroTributario", "MensagemPadraoNota", c => c.String(maxLength: 1000, unicode: false));
            DropColumn("dbo.OrdemVenda", "MensagemPadraoNota");
            DropColumn("dbo.NotaFiscal", "MensagemPadraoNota");
        }
    }
}
