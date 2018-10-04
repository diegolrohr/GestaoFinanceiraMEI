namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalItemTributacaoAddImpostosNFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalItemTributacao", "RetemCOFINS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "COFINSValorRetencao", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "RetemPIS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "PISValorRetencao", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaISS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ISSBase", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ISSAliquota", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ISSValor", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "RetemISS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ISSValorRetencao", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaCSLL", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CSLLBase", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CSLLAliquota", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CSLLValor", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "RetemCSLL", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CSLLValorRetencao", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaINSS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "INSSBase", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "INSSAliquota", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "INSSValor", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "RetemINSS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "INSSValorRetencao", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaImpostoRenda", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaBase", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaAliquota", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaValor", c => c.Double(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "RetemImpostoRenda", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaValorRetencao", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaValorRetencao");
            DropColumn("dbo.NotaFiscalItemTributacao", "RetemImpostoRenda");
            DropColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaValor");
            DropColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaAliquota");
            DropColumn("dbo.NotaFiscalItemTributacao", "ImpostoRendaBase");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaImpostoRenda");
            DropColumn("dbo.NotaFiscalItemTributacao", "INSSValorRetencao");
            DropColumn("dbo.NotaFiscalItemTributacao", "RetemINSS");
            DropColumn("dbo.NotaFiscalItemTributacao", "INSSValor");
            DropColumn("dbo.NotaFiscalItemTributacao", "INSSAliquota");
            DropColumn("dbo.NotaFiscalItemTributacao", "INSSBase");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaINSS");
            DropColumn("dbo.NotaFiscalItemTributacao", "CSLLValorRetencao");
            DropColumn("dbo.NotaFiscalItemTributacao", "RetemCSLL");
            DropColumn("dbo.NotaFiscalItemTributacao", "CSLLValor");
            DropColumn("dbo.NotaFiscalItemTributacao", "CSLLAliquota");
            DropColumn("dbo.NotaFiscalItemTributacao", "CSLLBase");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaCSLL");
            DropColumn("dbo.NotaFiscalItemTributacao", "ISSValorRetencao");
            DropColumn("dbo.NotaFiscalItemTributacao", "RetemISS");
            DropColumn("dbo.NotaFiscalItemTributacao", "ISSValor");
            DropColumn("dbo.NotaFiscalItemTributacao", "ISSAliquota");
            DropColumn("dbo.NotaFiscalItemTributacao", "ISSBase");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaISS");
            DropColumn("dbo.NotaFiscalItemTributacao", "PISValorRetencao");
            DropColumn("dbo.NotaFiscalItemTributacao", "RetemPIS");
            DropColumn("dbo.NotaFiscalItemTributacao", "COFINSValorRetencao");
            DropColumn("dbo.NotaFiscalItemTributacao", "RetemCOFINS");
        }
    }
}
