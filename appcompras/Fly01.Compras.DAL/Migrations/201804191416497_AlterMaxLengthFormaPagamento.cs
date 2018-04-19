namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterMaxLengthFormaPagamento : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FormaPagamento", "Descricao", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FormaPagamento", "Descricao", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
