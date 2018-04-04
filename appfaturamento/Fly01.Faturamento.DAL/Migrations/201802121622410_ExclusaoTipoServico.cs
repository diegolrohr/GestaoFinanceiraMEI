namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ExclusaoTipoServico : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Servico", "TipoServico");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servico", "TipoServico", c => c.Int(nullable: false));
        }
    }
}
