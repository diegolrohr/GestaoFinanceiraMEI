namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPlataformaBaseRegistroFixo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arquivo", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categoria", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.CertificadoDigital", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.CondicaoParcelamento", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.FormaPagamento", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoProduto", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItem", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscal", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pessoa", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdemVenda", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.SerieNotaFiscal", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Produto", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Servico", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdemVendaItem", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ParametroTributario", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.SubstituicaoTributaria", "RegistroFixo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubstituicaoTributaria", "RegistroFixo");
            DropColumn("dbo.ParametroTributario", "RegistroFixo");
            DropColumn("dbo.OrdemVendaItem", "RegistroFixo");
            DropColumn("dbo.NotaFiscalItemTributacao", "RegistroFixo");
            DropColumn("dbo.Servico", "RegistroFixo");
            DropColumn("dbo.Produto", "RegistroFixo");
            DropColumn("dbo.SerieNotaFiscal", "RegistroFixo");
            DropColumn("dbo.OrdemVenda", "RegistroFixo");
            DropColumn("dbo.Pessoa", "RegistroFixo");
            DropColumn("dbo.NotaFiscal", "RegistroFixo");
            DropColumn("dbo.NotaFiscalItem", "RegistroFixo");
            DropColumn("dbo.GrupoTributario", "RegistroFixo");
            DropColumn("dbo.GrupoProduto", "RegistroFixo");
            DropColumn("dbo.FormaPagamento", "RegistroFixo");
            DropColumn("dbo.CondicaoParcelamento", "RegistroFixo");
            DropColumn("dbo.CertificadoDigital", "RegistroFixo");
            DropColumn("dbo.Categoria", "RegistroFixo");
            DropColumn("dbo.Arquivo", "RegistroFixo");
        }
    }
}
