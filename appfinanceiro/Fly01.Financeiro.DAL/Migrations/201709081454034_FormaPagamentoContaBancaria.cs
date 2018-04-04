namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FormaPagamentoContaBancaria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormaPagamento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        TipoFormaPagamento = c.Int(nullable: false),
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
            
            AddColumn("dbo.ContaBancaria", "NomeConta", c => c.String(nullable: false, maxLength: 150, unicode: false));
            AddColumn("dbo.ContaBancaria", "DigitoAgencia", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ContaBancaria", "DigitoConta", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.ContaBancaria", "Saldo", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.ContaBancaria", "Descricao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContaBancaria", "Descricao", c => c.String(nullable: false, maxLength: 150, unicode: false));
            DropColumn("dbo.ContaBancaria", "Saldo");
            DropColumn("dbo.ContaBancaria", "DigitoConta");
            DropColumn("dbo.ContaBancaria", "DigitoAgencia");
            DropColumn("dbo.ContaBancaria", "NomeConta");
            DropTable("dbo.FormaPagamento");
        }
    }
}
