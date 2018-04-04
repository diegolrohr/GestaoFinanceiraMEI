namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterGrupoTributario : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoICMS", c => c.Int());
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoIPI", c => c.Int());
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoPIS", c => c.Int());
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoCOFINS", c => c.Int());
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoISS", c => c.Int());
            AlterColumn("dbo.GrupoTributario", "TipoPagamentoImpostoISS", c => c.Int());
            AlterColumn("dbo.GrupoTributario", "TipoCFPS", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GrupoTributario", "TipoCFPS", c => c.Int(nullable: false));
            AlterColumn("dbo.GrupoTributario", "TipoPagamentoImpostoISS", c => c.Int(nullable: false));
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoISS", c => c.Int(nullable: false));
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoCOFINS", c => c.Int(nullable: false));
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoPIS", c => c.Int(nullable: false));
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoIPI", c => c.Int(nullable: false));
            AlterColumn("dbo.GrupoTributario", "TipoTributacaoICMS", c => c.Int(nullable: false));
        }
    }
}
