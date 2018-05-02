namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCnab : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cnab",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NumeroBoleto = c.Int(nullable: false),
                        Status = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataEmissao = c.DateTime(nullable: false),
                        DataVencimento = c.DateTime(nullable: false),
                        NossoNumero = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataDesconto = c.DateTime(nullable: false),
                        ValorDesconto = c.Double(nullable: false),
                        ContaBancariaCedenteId = c.Guid(),
                        ContaReceberId = c.Guid(),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaCedenteId)
                .ForeignKey("dbo.ContaReceber", t => t.ContaReceberId)
                .Index(t => t.ContaBancariaCedenteId)
                .Index(t => t.ContaReceberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cnab", "ContaReceberId", "dbo.ContaReceber");
            DropForeignKey("dbo.Cnab", "ContaBancariaCedenteId", "dbo.ContaBancaria");
            DropIndex("dbo.Cnab", new[] { "ContaReceberId" });
            DropIndex("dbo.Cnab", new[] { "ContaBancariaCedenteId" });
            DropTable("dbo.Cnab");
        }
    }
}
