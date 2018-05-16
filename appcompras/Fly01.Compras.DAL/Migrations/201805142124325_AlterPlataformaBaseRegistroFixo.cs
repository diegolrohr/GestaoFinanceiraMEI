namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPlataformaBaseRegistroFixo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arquivo", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categoria", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.CondicaoParcelamento", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.FormaPagamento", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoProduto", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdemCompra", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pessoa", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Produto", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.SubstituicaoTributaria", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "RegistroFixo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemCompraItem", "RegistroFixo");
            DropColumn("dbo.SubstituicaoTributaria", "RegistroFixo");
            DropColumn("dbo.Produto", "RegistroFixo");
            DropColumn("dbo.Pessoa", "RegistroFixo");
            DropColumn("dbo.OrdemCompra", "RegistroFixo");
            DropColumn("dbo.GrupoTributario", "RegistroFixo");
            DropColumn("dbo.GrupoProduto", "RegistroFixo");
            DropColumn("dbo.FormaPagamento", "RegistroFixo");
            DropColumn("dbo.CondicaoParcelamento", "RegistroFixo");
            DropColumn("dbo.Categoria", "RegistroFixo");
            DropColumn("dbo.Arquivo", "RegistroFixo");
        }
    }
}
