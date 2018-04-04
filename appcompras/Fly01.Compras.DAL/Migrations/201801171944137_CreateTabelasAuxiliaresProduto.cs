namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateTabelasAuxiliaresProduto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GrupoProduto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        AliquotaIpi = c.Double(nullable: false),
                        TipoProduto = c.Int(nullable: false),
                        UnidadeMedidaId = c.Guid(),
                        NcmId = c.Guid(),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NCM", t => t.NcmId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.UnidadeMedidaId)
                .Index(t => t.NcmId);
            
            CreateTable(
                "dbo.NCM",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        AliquotaIPI = c.Double(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UnidadeMedida",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Abreviacao = c.String(nullable: false, maxLength: 2, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 20, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Produto", "GrupoProdutoId");
            CreateIndex("dbo.Produto", "UnidadeMedidaId");
            CreateIndex("dbo.Produto", "NcmId");
            AddForeignKey("dbo.Produto", "GrupoProdutoId", "dbo.GrupoProduto", "Id");
            AddForeignKey("dbo.Produto", "NcmId", "dbo.NCM", "Id");
            AddForeignKey("dbo.Produto", "UnidadeMedidaId", "dbo.UnidadeMedida", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.Produto", "NcmId", "dbo.NCM");
            DropForeignKey("dbo.Produto", "GrupoProdutoId", "dbo.GrupoProduto");
            DropForeignKey("dbo.GrupoProduto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.GrupoProduto", "NcmId", "dbo.NCM");
            DropIndex("dbo.Produto", new[] { "NcmId" });
            DropIndex("dbo.Produto", new[] { "UnidadeMedidaId" });
            DropIndex("dbo.Produto", new[] { "GrupoProdutoId" });
            DropIndex("dbo.GrupoProduto", new[] { "NcmId" });
            DropIndex("dbo.GrupoProduto", new[] { "UnidadeMedidaId" });
            DropTable("dbo.UnidadeMedida");
            DropTable("dbo.NCM");
            DropTable("dbo.GrupoProduto");
        }
    }
}
