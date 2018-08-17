namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSerieNotaFiscal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SerieNotaFiscal",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Serie = c.String(nullable: false, maxLength: 3, unicode: false),
                        TipoOperacaoSerieNotaFiscal = c.Int(),
                        NumNotaFiscal = c.Int(nullable: false),
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
            DropTable("dbo.SerieNotaFiscal");
        }
    }
}
