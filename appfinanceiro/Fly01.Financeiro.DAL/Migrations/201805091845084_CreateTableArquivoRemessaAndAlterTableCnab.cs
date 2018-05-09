namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableArquivoRemessaAndAlterTableCnab : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArquivoRemessa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NumeroArquivo = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        TotalBoletos = c.Int(nullable: false),
                        ValorTotal = c.Double(nullable: false),
                        StatusArquivoRemessa = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataExportacao = c.DateTime(nullable: false),
                        DataRetorno = c.DateTime(nullable: false),
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
            
            AddColumn("dbo.Cnab", "ArquivoRemessaId", c => c.Guid());
            CreateIndex("dbo.Cnab", "ArquivoRemessaId");
            AddForeignKey("dbo.Cnab", "ArquivoRemessaId", "dbo.ArquivoRemessa", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cnab", "ArquivoRemessaId", "dbo.ArquivoRemessa");
            DropIndex("dbo.Cnab", new[] { "ArquivoRemessaId" });
            DropColumn("dbo.Cnab", "ArquivoRemessaId");
            DropTable("dbo.ArquivoRemessa");
        }
    }
}
