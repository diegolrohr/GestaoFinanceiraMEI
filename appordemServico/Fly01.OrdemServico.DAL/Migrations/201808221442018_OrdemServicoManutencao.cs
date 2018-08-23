namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdemServicoManutencao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdemServicoManutencao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrdemServicoId = c.Guid(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                        Quantidade = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
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
                .ForeignKey("dbo.OrdemServico", t => t.OrdemServicoId)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .Index(t => t.OrdemServicoId)
                .Index(t => t.ProdutoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdemServicoManutencao", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.OrdemServicoManutencao", "OrdemServicoId", "dbo.OrdemServico");
            DropIndex("dbo.OrdemServicoManutencao", new[] { "ProdutoId" });
            DropIndex("dbo.OrdemServicoManutencao", new[] { "OrdemServicoId" });
            DropTable("dbo.OrdemServicoManutencao");
        }
    }
}
