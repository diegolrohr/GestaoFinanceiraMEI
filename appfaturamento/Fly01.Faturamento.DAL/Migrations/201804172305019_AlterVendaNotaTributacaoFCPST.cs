namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterVendaNotaTributacaoFCPST : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFeProduto", "ValorFCPSTRetidoAnterior", c => c.Double(nullable: false));
            AddColumn("dbo.NFeProduto", "AliquotaFCPConsumidorFinal", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "FCPSTBase", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "FCPSTAliquota", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "FCPSTValor", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemVendaProduto", "ValorFCPSTRetidoAnterior", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemVendaProduto", "AliquotaFCPConsumidorFinal", c => c.Double(nullable: false));
            AddColumn("dbo.SubstituicaoTributaria", "Fcp", c => c.Double(nullable: false));

            Sql("update dbo.NFeProduto set ValorCreditoICMS = isnull(ValorCreditoICMS, 0), ValorICMSSTRetido = isnull(ValorICMSSTRetido, 0), ValorBCSTRetido = isnull(ValorBCSTRetido, 0)");
            Sql("update dbo.OrdemVendaProduto set ValorCreditoICMS = isnull(ValorCreditoICMS, 0), ValorICMSSTRetido = isnull(ValorICMSSTRetido, 0), ValorBCSTRetido = isnull(ValorBCSTRetido, 0)");

            AlterColumn("dbo.NFeProduto", "ValorCreditoICMS", c => c.Double(nullable: false, defaultValue: 0));
            AlterColumn("dbo.NFeProduto", "ValorICMSSTRetido", c => c.Double(nullable: false, defaultValue: 0));
            AlterColumn("dbo.NFeProduto", "ValorBCSTRetido", c => c.Double(nullable: false, defaultValue: 0));
            AlterColumn("dbo.OrdemVendaProduto", "ValorCreditoICMS", c => c.Double(nullable: false, defaultValue: 0));
            AlterColumn("dbo.OrdemVendaProduto", "ValorICMSSTRetido", c => c.Double(nullable: false, defaultValue: 0));
            AlterColumn("dbo.OrdemVendaProduto", "ValorBCSTRetido", c => c.Double(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdemVendaProduto", "ValorBCSTRetido", c => c.Double());
            AlterColumn("dbo.OrdemVendaProduto", "ValorICMSSTRetido", c => c.Double());
            AlterColumn("dbo.OrdemVendaProduto", "ValorCreditoICMS", c => c.Double());
            AlterColumn("dbo.NFeProduto", "ValorBCSTRetido", c => c.Double());
            AlterColumn("dbo.NFeProduto", "ValorICMSSTRetido", c => c.Double());
            AlterColumn("dbo.NFeProduto", "ValorCreditoICMS", c => c.Double());
            DropColumn("dbo.SubstituicaoTributaria", "Fcp");
            DropColumn("dbo.OrdemVendaProduto", "AliquotaFCPConsumidorFinal");
            DropColumn("dbo.OrdemVendaProduto", "ValorFCPSTRetidoAnterior");
            DropColumn("dbo.NotaFiscalItemTributacao", "FCPSTValor");
            DropColumn("dbo.NotaFiscalItemTributacao", "FCPSTAliquota");
            DropColumn("dbo.NotaFiscalItemTributacao", "FCPSTBase");
            DropColumn("dbo.NFeProduto", "AliquotaFCPConsumidorFinal");
            DropColumn("dbo.NFeProduto", "ValorFCPSTRetidoAnterior");
        }
    }
}
