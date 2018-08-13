namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateArquivo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arquivo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(maxLength: 50, unicode: false),
                        Conteudo = c.String(unicode: false),
                        Md5 = c.String(maxLength: 200, unicode: false),
                        Cadastro = c.String(maxLength: 30, unicode: false),
                        TotalProcessado = c.Double(nullable: false),
                        Retorno = c.String(unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
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
            DropTable("dbo.Arquivo");
        }
    }
}
