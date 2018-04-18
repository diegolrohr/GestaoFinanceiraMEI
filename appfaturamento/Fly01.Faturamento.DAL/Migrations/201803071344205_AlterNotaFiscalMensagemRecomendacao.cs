namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterNotaFiscalMensagemRecomendacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "Mensagem", c => c.String(unicode: false));
            AddColumn("dbo.NotaFiscal", "Recomendacao", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscal", "Recomendacao");
            DropColumn("dbo.NotaFiscal", "Mensagem");
        }
    }
}
