namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cidade",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 35, unicode: false),
                        CodigoIbge = c.String(nullable: false, maxLength: 7, unicode: false),
                        EstadoId = c.Guid(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .Index(t => t.EstadoId);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sigla = c.String(nullable: false, maxLength: 2, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        UtcId = c.String(nullable: false, maxLength: 35, unicode: false),
                        CodigoIbge = c.String(maxLength: 2, unicode: false),
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
                "dbo.CondicaoParcelamento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        QtdParcelas = c.Int(),
                        CondicoesParcelamento = c.String(maxLength: 200, unicode: false),
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
            
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        TipoDocumento = c.String(nullable: false, maxLength: 1, unicode: false),
                        CPFCNPJ = c.String(nullable: false, maxLength: 18, unicode: false),
                        CEP = c.String(maxLength: 8, unicode: false),
                        Endereco = c.String(maxLength: 50, unicode: false),
                        Numero = c.String(maxLength: 20, unicode: false),
                        Complemento = c.String(maxLength: 20, unicode: false),
                        Bairro = c.String(maxLength: 30, unicode: false),
                        CidadeId = c.Guid(),
                        EstadoId = c.Guid(),
                        Telefone = c.String(maxLength: 15, unicode: false),
                        Celular = c.String(maxLength: 15, unicode: false),
                        Contato = c.String(maxLength: 45, unicode: false),
                        Observacao = c.String(maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 70, unicode: false),
                        NomeComercial = c.String(nullable: false, maxLength: 100, unicode: false),
                        Transportadora = c.Boolean(nullable: false),
                        Cliente = c.Boolean(nullable: false),
                        Fornecedor = c.Boolean(nullable: false),
                        Vendedor = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pessoa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Pessoa", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.Cidade", "EstadoId", "dbo.Estado");
            DropIndex("dbo.Pessoa", new[] { "EstadoId" });
            DropIndex("dbo.Pessoa", new[] { "CidadeId" });
            DropIndex("dbo.Cidade", new[] { "EstadoId" });
            DropTable("dbo.Pessoa");
            DropTable("dbo.CondicaoParcelamento");
            DropTable("dbo.Estado");
            DropTable("dbo.Cidade");
        }
    }
}
