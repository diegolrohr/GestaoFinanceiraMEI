namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateCertificadoDigtal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CertificadoDigital",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Tipo = c.Int(nullable: false),
                        DataEmissao = c.DateTime(nullable: false),
                        DataExpiracao = c.DateTime(nullable: false),
                        Chave = c.String(maxLength: 200, unicode: false),
                        Certificado = c.String(nullable: false, maxLength: 200, unicode: false),
                        Senha = c.String(maxLength: 200, unicode: false),
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
            DropTable("dbo.CertificadoDigital");
        }
    }
}
