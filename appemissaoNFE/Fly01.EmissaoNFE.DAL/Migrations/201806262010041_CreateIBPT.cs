namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIBPT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IbptNcm",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        ImpostoNacional = c.Double(nullable: false),
                        ImpostoEstadual = c.Double(nullable: false),
                        ImpostoMunicipal = c.Double(nullable: false),
                        ImpostoImportacao = c.Double(nullable: false),
                        Versao = c.String(maxLength: 200, unicode: false),
                        UF = c.String(maxLength: 200, unicode: false),
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
            DropTable("dbo.IbptNcm");
        }
    }
}
