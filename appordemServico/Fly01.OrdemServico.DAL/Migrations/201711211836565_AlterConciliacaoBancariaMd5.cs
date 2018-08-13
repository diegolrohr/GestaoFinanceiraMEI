namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterConciliacaoBancariaMd5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConciliacaoBancariaItem", "OfxLancamentoMD5", c => c.String(nullable: false, maxLength: 32, unicode: false));
            AddColumn("dbo.ConciliacaoBancariaItem", "ConciliadoTotal", c => c.Boolean(nullable: false));
            DropColumn("dbo.ConciliacaoBancaria", "Status");
            DropColumn("dbo.ConciliacaoBancaria", "ArquivoMD5");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConciliacaoBancaria", "ArquivoMD5", c => c.String(nullable: false, maxLength: 32, unicode: false));
            AddColumn("dbo.ConciliacaoBancaria", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.ConciliacaoBancariaItem", "ConciliadoTotal");
            DropColumn("dbo.ConciliacaoBancariaItem", "OfxLancamentoMD5");
        }
    }
}
