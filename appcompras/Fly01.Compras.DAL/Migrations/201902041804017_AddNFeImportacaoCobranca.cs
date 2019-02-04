namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNFeImportacaoCobranca : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NFeImportacaoCobranca",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NFeImportacaoId = c.Guid(nullable: false),
                        Numero = c.String(maxLength: 60, unicode: false),
                        Valor = c.Double(nullable: false),
                        DataVencimento = c.DateTime(nullable: false),
                        ContaFinanceiraId = c.Guid(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NFeImportacao", t => t.NFeImportacaoId)
                .Index(t => t.NFeImportacaoId);
            
            AddColumn("dbo.NFeImportacao", "GeraContasXml", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NFeImportacaoCobranca", "NFeImportacaoId", "dbo.NFeImportacao");
            DropIndex("dbo.NFeImportacaoCobranca", new[] { "NFeImportacaoId" });
            DropColumn("dbo.NFeImportacao", "GeraContasXml");
            DropTable("dbo.NFeImportacaoCobranca");
        }
    }
}
