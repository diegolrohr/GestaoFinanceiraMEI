namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCNABArquivoRemessa : DbMigration
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
            
            CreateTable(
                "dbo.Cnab",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NumeroBoleto = c.Int(nullable: false),
                        Status = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataEmissao = c.DateTime(nullable: false, storeType: "date"),
                        DataVencimento = c.DateTime(nullable: false, storeType: "date"),
                        NossoNumero = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataDesconto = c.DateTime(nullable: false, storeType: "date"),
                        ValorDesconto = c.Double(nullable: false),
                        ValorBoleto = c.Double(nullable: false),
                        ContaBancariaCedenteId = c.Guid(),
                        ContaReceberId = c.Guid(),
                        ArquivoRemessaId = c.Guid(),
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
                .ForeignKey("dbo.ArquivoRemessa", t => t.ArquivoRemessaId)
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaCedenteId)
                .ForeignKey("dbo.ContaReceber", t => t.ContaReceberId)
                .Index(t => t.ContaBancariaCedenteId)
                .Index(t => t.ContaReceberId)
                .Index(t => t.ArquivoRemessaId);
            
            AddColumn("dbo.Banco", "EmiteBoleto", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContaFinanceira", "DataDesconto", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.ContaFinanceira", "ValorDesconto", c => c.Double());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cnab", "ContaReceberId", "dbo.ContaReceber");
            DropForeignKey("dbo.Cnab", "ContaBancariaCedenteId", "dbo.ContaBancaria");
            DropForeignKey("dbo.Cnab", "ArquivoRemessaId", "dbo.ArquivoRemessa");
            DropIndex("dbo.Cnab", new[] { "ArquivoRemessaId" });
            DropIndex("dbo.Cnab", new[] { "ContaReceberId" });
            DropIndex("dbo.Cnab", new[] { "ContaBancariaCedenteId" });
            DropColumn("dbo.ContaFinanceira", "ValorDesconto");
            DropColumn("dbo.ContaFinanceira", "DataDesconto");
            DropColumn("dbo.Banco", "EmiteBoleto");
            DropTable("dbo.Cnab");
            DropTable("dbo.ArquivoRemessa");
        }
    }
}
