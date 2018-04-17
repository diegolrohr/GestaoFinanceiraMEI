namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NfeFcp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalItemTributacao", "ValorBaseFCPRetidoST", c => c.Double());
            AddColumn("dbo.NotaFiscalItemTributacao", "PercentualFCPRetidoST", c => c.Double());
            AddColumn("dbo.NotaFiscalItemTributacao", "ValorFCPST", c => c.Double());
            AddColumn("dbo.NotaFiscalItemTributacao", "AliquotaFCPConsumidorFinal", c => c.Double());
            AddColumn("dbo.NotaFiscalItemTributacao", "ValorBaseFCPRetidoAnteriorST", c => c.Double());
            AddColumn("dbo.NotaFiscalItemTributacao", "PercentualFCPRetidoAnteriorST", c => c.Double());
            AddColumn("dbo.NotaFiscalItemTributacao", "ValorFCPRetidoST", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscalItemTributacao", "ValorFCPRetidoST");
            DropColumn("dbo.NotaFiscalItemTributacao", "PercentualFCPRetidoAnteriorST");
            DropColumn("dbo.NotaFiscalItemTributacao", "ValorBaseFCPRetidoAnteriorST");
            DropColumn("dbo.NotaFiscalItemTributacao", "AliquotaFCPConsumidorFinal");
            DropColumn("dbo.NotaFiscalItemTributacao", "ValorFCPST");
            DropColumn("dbo.NotaFiscalItemTributacao", "PercentualFCPRetidoST");
            DropColumn("dbo.NotaFiscalItemTributacao", "ValorBaseFCPRetidoST");
        }
    }
}
