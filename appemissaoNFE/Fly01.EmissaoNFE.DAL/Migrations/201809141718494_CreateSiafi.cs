namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSiafi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Siafi",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CodigoIbge = c.String(nullable: false, maxLength: 7, unicode: false),
                        CodigoSiafi = c.String(nullable: false, maxLength: 7, unicode: false),
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
            DropTable("dbo.Siafi");
        }
    }
}
