namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterDataContaFinanceiraBaixa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContaFinanceiraBaixa", "Data", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContaFinanceiraBaixa", "Data", c => c.DateTime(nullable: false));
        }
    }
}
