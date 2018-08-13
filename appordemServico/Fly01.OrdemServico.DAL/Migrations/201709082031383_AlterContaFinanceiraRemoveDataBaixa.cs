namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterContaFinanceiraRemoveDataBaixa : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ContaFinanceira", "DataBaixa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContaFinanceira", "DataBaixa", c => c.DateTime(storeType: "date"));
        }
    }
}
