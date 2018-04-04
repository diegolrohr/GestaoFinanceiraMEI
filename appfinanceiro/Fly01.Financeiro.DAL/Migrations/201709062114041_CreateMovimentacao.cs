namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateMovimentacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movimentacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        Valor = c.Double(nullable: false),
                        ContaBancariaOrigemId = c.Guid(),
                        ContaBancariaDestinoId = c.Guid(),
                        ContaFinanceiraId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaDestinoId)
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaOrigemId)
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraId)
                .Index(t => t.ContaBancariaOrigemId)
                .Index(t => t.ContaBancariaDestinoId)
                .Index(t => t.ContaFinanceiraId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movimentacao", "ContaFinanceiraId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.Movimentacao", "ContaBancariaOrigemId", "dbo.ContaBancaria");
            DropForeignKey("dbo.Movimentacao", "ContaBancariaDestinoId", "dbo.ContaBancaria");
            DropIndex("dbo.Movimentacao", new[] { "ContaFinanceiraId" });
            DropIndex("dbo.Movimentacao", new[] { "ContaBancariaDestinoId" });
            DropIndex("dbo.Movimentacao", new[] { "ContaBancariaOrigemId" });
            DropTable("dbo.Movimentacao");
        }
    }
}
