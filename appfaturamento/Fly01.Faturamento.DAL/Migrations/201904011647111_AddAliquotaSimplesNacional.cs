namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAliquotaSimplesNacional : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AliquotaSimplesNacional",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TipoFaixaReceitaBruta = c.Int(nullable: false),
                        TipoEnquadramentoEmpresa = c.Int(nullable: false),
                        SimplesNacional = c.Double(nullable: false),
                        ImpostoRenda = c.Double(nullable: false),
                        Csll = c.Double(nullable: false),
                        Cofins = c.Double(nullable: false),
                        PisPasep = c.Double(nullable: false),
                        Ipi = c.Double(nullable: false),
                        Iss = c.Double(nullable: false),
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
            DropTable("dbo.AliquotaSimplesNacional");
        }
    }
}
