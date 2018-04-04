namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterContaBancariaRemoveSaldo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ContaBancaria", "Saldo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContaBancaria", "Saldo", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
