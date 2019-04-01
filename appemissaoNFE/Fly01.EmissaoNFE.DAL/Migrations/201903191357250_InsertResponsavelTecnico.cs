namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertResponsavelTecnico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResponsavelTecnico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CNPJ = c.String(maxLength: 200, unicode: false),
                        Contato = c.String(maxLength: 200, unicode: false),
                        Email = c.String(maxLength: 200, unicode: false),
                        Fone = c.String(maxLength: 200, unicode: false),
                        IdentificadorCodigoResponsavelTecnico = c.String(maxLength: 200, unicode: false),
                        CodigoResponsavelTecnico = c.String(maxLength: 200, unicode: false),
                        HashCSRT = c.String(maxLength: 200, unicode: false),
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
            DropTable("dbo.ResponsavelTecnico");
        }
    }
}
