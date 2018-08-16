namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCertificadoDigital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CertificadoDigital",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Tipo = c.Int(nullable: false),
                        DataEmissao = c.DateTime(),
                        DataExpiracao = c.DateTime(),
                        Certificado = c.String(nullable: false, unicode: false),
                        Senha = c.String(nullable: false, maxLength: 200, unicode: false),
                        EntidadeHomologacao = c.String(maxLength: 6, unicode: false),
                        EntidadeProducao = c.String(maxLength: 6, unicode: false),
                        Versao = c.String(maxLength: 30, unicode: false),
                        Emissor = c.String(maxLength: 200, unicode: false),
                        Pessoa = c.String(maxLength: 200, unicode: false),
                        Md5 = c.String(nullable: false, maxLength: 32, unicode: false),
                        Cnpj = c.String(maxLength: 16, unicode: false),
                        InscricaoEstadual = c.String(maxLength: 18, unicode: false),
                        UF = c.String(maxLength: 2, unicode: false),
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
            DropTable("dbo.CertificadoDigital");
        }
    }
}
