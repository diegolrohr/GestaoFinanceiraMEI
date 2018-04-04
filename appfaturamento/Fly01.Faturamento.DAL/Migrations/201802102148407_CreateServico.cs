namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateServico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NBS",
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
            
            CreateTable(
                "dbo.Servico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CodigoServico = c.String(maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 200, unicode: false),
                        NbsId = c.Guid(),
                        ValorServico = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        TipoServico = c.Int(nullable: false),
                        TipoPagamentoImpostoISS = c.Int(nullable: false),
                        TipoTributacaoISS = c.Int(nullable: false),
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
                .ForeignKey("dbo.NBS", t => t.NbsId)
                .Index(t => t.NbsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servico", "NbsId", "dbo.NBS");
            DropIndex("dbo.Servico", new[] { "NbsId" });
            DropTable("dbo.Servico");
            DropTable("dbo.NBS");
        }
    }
}
