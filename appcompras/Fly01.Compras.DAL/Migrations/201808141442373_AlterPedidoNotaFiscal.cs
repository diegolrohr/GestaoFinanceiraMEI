namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPedidoNotaFiscal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrdemCompraItem", "GrupoTributarioId", "dbo.GrupoTributario");
            DropIndex("dbo.OrdemCompraItem", new[] { "GrupoTributarioId" });
            AddColumn("dbo.NotaFiscalEntrada", "NumeracaoVolumesTrans", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.NotaFiscalEntrada", "Marca", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.NotaFiscalEntrada", "TipoCompra", c => c.Int(nullable: false));
            AddColumn("dbo.NotaFiscalEntrada", "TipoEspecie", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.OrdemCompra", "TipoCompra", c => c.Int(nullable: false));
            AddColumn("dbo.PedidoItem", "GrupoTributarioId", c => c.Guid(nullable: false));
            AddColumn("dbo.PedidoItem", "ValorCreditoICMS", c => c.Double(nullable: false));
            AddColumn("dbo.PedidoItem", "ValorICMSSTRetido", c => c.Double(nullable: false));
            AddColumn("dbo.PedidoItem", "ValorBCSTRetido", c => c.Double(nullable: false));
            AddColumn("dbo.PedidoItem", "ValorFCPSTRetidoAnterior", c => c.Double(nullable: false));
            AddColumn("dbo.PedidoItem", "ValorBCFCPSTRetidoAnterior", c => c.Double(nullable: false));
            CreateIndex("dbo.PedidoItem", "GrupoTributarioId");
            DropColumn("dbo.NotaFiscalEntrada", "TipoVenda");
            DropColumn("dbo.OrdemCompra", "TipoVenda");
            DropColumn("dbo.OrdemCompra", "TotalImpostosServicos");
            DropColumn("dbo.OrdemCompraItem", "GrupoTributarioId");
            DropColumn("dbo.OrdemCompraItem", "ValorCreditoICMS");
            DropColumn("dbo.OrdemCompraItem", "ValorICMSSTRetido");
            DropColumn("dbo.OrdemCompraItem", "ValorBCSTRetido");
            DropColumn("dbo.OrdemCompraItem", "ValorFCPSTRetidoAnterior");
            DropColumn("dbo.OrdemCompraItem", "ValorBCFCPSTRetidoAnterior");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemCompraItem", "ValorBCFCPSTRetidoAnterior", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorFCPSTRetidoAnterior", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorBCSTRetido", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorICMSSTRetido", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorCreditoICMS", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "GrupoTributarioId", c => c.Guid(nullable: false));
            AddColumn("dbo.OrdemCompra", "TotalImpostosServicos", c => c.Double());
            AddColumn("dbo.OrdemCompra", "TipoVenda", c => c.Int(nullable: false));
            AddColumn("dbo.NotaFiscalEntrada", "TipoVenda", c => c.Int(nullable: false));
            DropIndex("dbo.PedidoItem", new[] { "GrupoTributarioId" });
            DropColumn("dbo.PedidoItem", "ValorBCFCPSTRetidoAnterior");
            DropColumn("dbo.PedidoItem", "ValorFCPSTRetidoAnterior");
            DropColumn("dbo.PedidoItem", "ValorBCSTRetido");
            DropColumn("dbo.PedidoItem", "ValorICMSSTRetido");
            DropColumn("dbo.PedidoItem", "ValorCreditoICMS");
            DropColumn("dbo.PedidoItem", "GrupoTributarioId");
            DropColumn("dbo.OrdemCompra", "TipoCompra");
            DropColumn("dbo.NotaFiscalEntrada", "TipoEspecie");
            DropColumn("dbo.NotaFiscalEntrada", "TipoCompra");
            DropColumn("dbo.NotaFiscalEntrada", "Marca");
            DropColumn("dbo.NotaFiscalEntrada", "NumeracaoVolumesTrans");
            CreateIndex("dbo.OrdemCompraItem", "GrupoTributarioId");
        }
    }
}
