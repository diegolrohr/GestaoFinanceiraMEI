namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContaFinanceiraDependencias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        CategoriaPaiId = c.Guid(),
                        TipoCarteira = c.Int(nullable: false),
                        EmpresaId = c.Guid(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaPaiId)
                .Index(t => t.CategoriaPaiId);
            
            CreateTable(
                "dbo.CondicaoParcelamento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        QtdParcelas = c.Int(),
                        CondicoesParcelamento = c.String(maxLength: 200, unicode: false),
                        EmpresaId = c.Guid(nullable: false),
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
                "dbo.ContaBancaria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NomeConta = c.String(nullable: false, maxLength: 150, unicode: false),
                        Agencia = c.String(maxLength: 200, unicode: false),
                        DigitoAgencia = c.String(maxLength: 200, unicode: false),
                        Conta = c.String(maxLength: 200, unicode: false),
                        DigitoConta = c.String(maxLength: 200, unicode: false),
                        BancoId = c.Guid(nullable: false),
                        ValorInicial = c.Double(),
                        EmpresaId = c.Guid(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banco", t => t.BancoId)
                .Index(t => t.BancoId);
            
            CreateTable(
                "dbo.FormaPagamento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        TipoFormaPagamento = c.Int(nullable: false),
                        EmpresaId = c.Guid(nullable: false),
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
            DropForeignKey("dbo.ContaBancaria", "BancoId", "dbo.Banco");
            DropForeignKey("dbo.Categoria", "CategoriaPaiId", "dbo.Categoria");
            DropIndex("dbo.ContaBancaria", new[] { "BancoId" });
            DropIndex("dbo.Categoria", new[] { "CategoriaPaiId" });
            DropTable("dbo.FormaPagamento");
            DropTable("dbo.ContaBancaria");
            DropTable("dbo.CondicaoParcelamento");
            DropTable("dbo.Categoria");
        }
    }
}
