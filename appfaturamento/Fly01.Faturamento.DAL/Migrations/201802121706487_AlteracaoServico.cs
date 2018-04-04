namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoServico : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Servico", "TipoPagamentoImpostoISS", c => c.Int());
            AlterColumn("dbo.Servico", "TipoTributacaoISS", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Servico", "TipoTributacaoISS", c => c.Int(nullable: false));
            AlterColumn("dbo.Servico", "TipoPagamentoImpostoISS", c => c.Int(nullable: false));
        }
    }
}
