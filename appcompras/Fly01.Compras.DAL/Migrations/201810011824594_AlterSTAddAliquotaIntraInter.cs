namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AlterSTAddAliquotaIntraInter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubstituicaoTributaria", "AliquotaIntraEstadual", c => c.Double(nullable: false, defaultValue: 0));
            AddColumn("dbo.SubstituicaoTributaria", "AliquotaInterEstadual", c => c.Double(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubstituicaoTributaria", "AliquotaInterEstadual");
            DropColumn("dbo.SubstituicaoTributaria", "AliquotaIntraEstadual");
        }
    }
}
