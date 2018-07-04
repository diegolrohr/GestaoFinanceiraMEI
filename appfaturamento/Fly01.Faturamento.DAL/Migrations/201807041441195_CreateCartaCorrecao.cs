namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCartaCorrecao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotaFiscalCartaCorrecao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotaFiscalId = c.Guid(nullable: false),
                        IdRetorno = c.String(maxLength: 200, unicode: false),
                        Data = c.DateTime(nullable: false),
                        MensagemCorrecao = c.String(nullable: false, maxLength: 1000, unicode: false),
                        Mensagem = c.String(unicode: false),
                        XML = c.String(unicode: false),
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
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId)
                .Index(t => t.NotaFiscalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscalCartaCorrecao", "NotaFiscalId", "dbo.NotaFiscal");
            DropIndex("dbo.NotaFiscalCartaCorrecao", new[] { "NotaFiscalId" });
            DropTable("dbo.NotaFiscalCartaCorrecao");
        }
    }
}
