namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_EnquadramentoLegalIPI : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EnquadramentoLegalIPI",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(maxLength: 200, unicode: false),
                        GrupoCST = c.String(maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 600, unicode: false),
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
            DropTable("dbo.EnquadramentoLegalIPI");
        }
    }
}
