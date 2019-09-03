namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPessoa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 180, unicode: false),
                        TipoDocumento = c.String(maxLength: 1, unicode: false),
                        CPFCNPJ = c.String(maxLength: 18, unicode: false),
                        CEP = c.String(maxLength: 8, unicode: false),
                        Endereco = c.String(maxLength: 180, unicode: false),
                        Numero = c.String(maxLength: 60, unicode: false),
                        Complemento = c.String(maxLength: 500, unicode: false),
                        Bairro = c.String(maxLength: 200, unicode: false),
                        CidadeId = c.Guid(),
                        EstadoId = c.Guid(),
                        Telefone = c.String(maxLength: 15, unicode: false),
                        Celular = c.String(maxLength: 15, unicode: false),
                        Contato = c.String(maxLength: 60, unicode: false),
                        Observacao = c.String(maxLength: 500, unicode: false),
                        Email = c.String(maxLength: 100, unicode: false),
                        NomeComercial = c.String(maxLength: 180, unicode: false),
                        Cliente = c.Boolean(nullable: false),
                        Fornecedor = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pessoa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Pessoa", "CidadeId", "dbo.Cidade");
            DropIndex("dbo.Pessoa", new[] { "EstadoId" });
            DropIndex("dbo.Pessoa", new[] { "CidadeId" });
            DropTable("dbo.Pessoa");
        }
    }
}
