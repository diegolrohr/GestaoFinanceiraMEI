namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKitKitItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KitItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        KitId = c.Guid(nullable: false),
                        ProdutoId = c.Guid(),
                        ServicoId = c.Guid(),
                        Quantidade = c.Double(nullable: false),
                        TipoItem = c.Int(nullable: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kit", t => t.KitId)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .ForeignKey("dbo.Servico", t => t.ServicoId)
                .Index(t => t.KitId)
                .Index(t => t.ProdutoId)
                .Index(t => t.ServicoId);
            
            CreateTable(
                "dbo.Kit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KitItem", "ServicoId", "dbo.Servico");
            DropForeignKey("dbo.KitItem", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.KitItem", "KitId", "dbo.Kit");
            DropIndex("dbo.KitItem", new[] { "ServicoId" });
            DropIndex("dbo.KitItem", new[] { "ProdutoId" });
            DropIndex("dbo.KitItem", new[] { "KitId" });
            DropTable("dbo.Kit");
            DropTable("dbo.KitItem");
        }
    }
}
