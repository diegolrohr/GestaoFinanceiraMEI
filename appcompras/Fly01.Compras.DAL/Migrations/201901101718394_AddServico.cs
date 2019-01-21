namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Servico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CodigoServico = c.String(maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 200, unicode: false),
                        NbsId = c.Guid(),
                        IssId = c.Guid(),
                        CodigoTributacaoMunicipal = c.String(maxLength: 20, unicode: false),
                        ValorServico = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        CodigoIssEspecifico = c.String(maxLength: 20, unicode: false),
                        CodigoFiscalPrestacao = c.String(maxLength: 5, unicode: false),
                        CodigoIss = c.String(maxLength: 200, unicode: false),
                        CodigoNbs = c.String(maxLength: 200, unicode: false),
                        AbreviacaoUnidadeMedida = c.String(maxLength: 200, unicode: false),
                        UnidadeMedidaId = c.Guid(),
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
                .ForeignKey("dbo.Iss", t => t.IssId)
                .ForeignKey("dbo.Nbs", t => t.NbsId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.NbsId)
                .Index(t => t.IssId)
                .Index(t => t.UnidadeMedidaId);
            
            CreateTable(
                "dbo.Iss",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
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
                "dbo.Nbs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 600, unicode: false),
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
            DropForeignKey("dbo.Servico", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.Servico", "NbsId", "dbo.Nbs");
            DropForeignKey("dbo.Servico", "IssId", "dbo.Iss");
            DropIndex("dbo.Servico", new[] { "UnidadeMedidaId" });
            DropIndex("dbo.Servico", new[] { "IssId" });
            DropIndex("dbo.Servico", new[] { "NbsId" });
            DropTable("dbo.Nbs");
            DropTable("dbo.Iss");
            DropTable("dbo.Servico");
        }
    }
}
